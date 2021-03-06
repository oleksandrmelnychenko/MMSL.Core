﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MMSL.Common;
using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.Helpers;
using MMSL.Common.IdentityConfiguration;
using MMSL.Common.WebApi;
using MMSL.Common.WebApi.RoutingConfiguration;
using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.Identity;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.Repositories.Identity.Contracts;
using MMSL.Services.IdentityServices.Contracts;


namespace MMSL.Services.IdentityServices {

    public class UserIdentityService : IUserIdentityService {
        private readonly IIdentityRepositoriesFactory _identityRepositoriesFactory;
        private readonly IDbConnectionFactory _connectionFactory;

        public UserIdentityService(
            IDbConnectionFactory connectionFactory,
            IIdentityRepositoriesFactory identityRepositoriesFactory) {
            _identityRepositoriesFactory = identityRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<UserAccount> SignInAsync(AuthenticationDataContract authenticateDataContract) =>
              Task.Run(() => {
                  if (!Validator.IsEmailValid(authenticateDataContract.Email))
                      UserExceptionCreator<InvalidIdentityException>.Create(
                          IdentityValidationMessages.EMAIL_INVALID,
                          SignInErrorResponseModel.New(SignInErrorResponseType.InvalidEmail,
                              IdentityValidationMessages.EMAIL_INVALID)).Throw();

                  using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                      IIdentityRepository repository = _identityRepositoriesFactory.NewIdentityRepository(connection);
                      UserIdentity user = repository.GetUserByEmail(authenticateDataContract.Email);

                      if (user == null) {
                          UserExceptionCreator<InvalidIdentityException>.Create(
                              IdentityValidationMessages.INVALID_CREDENTIALS,
                              SignInErrorResponseModel.New(SignInErrorResponseType.InvalidCredentials,
                                  IdentityValidationMessages.INVALID_CREDENTIALS)).Throw();
                      }

                      if (user.IsDeleted) {
                          UserExceptionCreator<InvalidIdentityException>.Create(
                              IdentityValidationMessages.USER_DELETED,
                              SignInErrorResponseModel.New(SignInErrorResponseType.UserDeleted,
                                  IdentityValidationMessages.USER_DELETED)).Throw();
                      }

                      if (!CryptoHelper.Validate(authenticateDataContract.Password, user.PasswordSalt, user.PasswordHash)) {
                          UserExceptionCreator<InvalidIdentityException>.Create(
                              IdentityValidationMessages.INVALID_CREDENTIALS,
                              SignInErrorResponseModel.New(SignInErrorResponseType.InvalidCredentials,
                                  IdentityValidationMessages.INVALID_CREDENTIALS)).Throw();
                      }

                      byte[] key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings.TokenSecret);
                      DateTime expiry = DateTime.UtcNow.AddDays(ConfigurationManager.AppSettings.TokenExpiryDays);
                      ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                          {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Expiration, expiry.Ticks.ToString())
                          }
                      );

                      foreach (var userRole in user.UserRoles) {
                          claims.AddClaim(new Claim(ClaimTypes.Role, userRole.UserRoleType.RoleType.ToString()));
                      }
                      claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                      SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
                          Issuer = AuthOptions.ISSUER,
                          Audience = AuthOptions.AUDIENCE_LOCAL,
                          Subject = claims,
                          Expires = expiry,
                          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                              SecurityAlgorithms.HmacSha256Signature)
                      };

                      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                      JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                      UserAccount userData = new UserAccount(user) {
                          TokenExpiresAt = expiry,
                          Token = tokenHandler.WriteToken(token),
                      };

                      if (IsUserPasswordExpired(user)) {
                          repository.UpdateUserExperationDate(user.Id, true);
                          UserExceptionCreator<InvalidIdentityException>.Create(
                              IdentityValidationMessages.PASSWORD_EXPIRED,
                              SignInErrorResponseModel.New(SignInErrorResponseType.PasswordExpired,
                                  userData.CanUserResetExpiredPassword
                                      ? IdentityValidationMessages.PASSWORD_EXPIRED
                                      : IdentityValidationMessages.PASSWORD_EXPIRED_PLEASE_RESET, userData)).Throw();
                      } else {
                          user.LastLoggedIn = DateTime.Now;
                          userData.LastLoggedIn = user.LastLoggedIn;

                          repository.UpdateUserLastLoggedInDate(user.Id, user.LastLoggedIn.Value);
                      }

                      return userData;
                  }
              });

        public Task<UserAccount> ValidateToken(ClaimsPrincipal userPrincipal) =>
            Task.Run(() => {
                long userId = long.Parse(userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
                string email = userPrincipal.FindFirstValue(ClaimTypes.Email);

                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IIdentityRepository repository = _identityRepositoriesFactory.NewIdentityRepository(connection);

                    UserIdentity currentUser = repository.GetUserById(userId);

                    if (currentUser == null) {
                        UserExceptionCreator<InvalidIdentityException>.Create(
                            IdentityValidationMessages.TOKEN_INVALID,
                            SignInErrorResponseModel.New(SignInErrorResponseType.InvalidToken,
                                IdentityValidationMessages.TOKEN_INVALID)).Throw();
                    }

                    if (currentUser.IsDeleted) {
                        UserExceptionCreator<InvalidIdentityException>.Create(
                            IdentityValidationMessages.USER_DELETED,
                            SignInErrorResponseModel.New(SignInErrorResponseType.UserDeleted,
                                IdentityValidationMessages.USER_DELETED)).Throw();
                    }

                    if (email == null || !email.Equals(currentUser.Email, StringComparison.OrdinalIgnoreCase)) {
                        UserExceptionCreator<InvalidIdentityException>.Create(
                            IdentityValidationMessages.EMAIL_INVALID,
                            SignInErrorResponseModel.New(SignInErrorResponseType.InvalidEmail,
                                IdentityValidationMessages.EMAIL_INVALID)).Throw();
                    }

                    currentUser = repository.GetUserByEmail(email);

                    if (IsUserPasswordExpired(currentUser)) {
                        UserExceptionCreator<InvalidIdentityException>.Create(
                            IdentityValidationMessages.PASSWORD_EXPIRED,
                            SignInErrorResponseModel.New(SignInErrorResponseType.PasswordExpired,
                                IdentityValidationMessages.PASSWORD_EXPIRED)).Throw();
                    }

                    DateTime tokenExpiresAt = new DateTime(long.Parse(userPrincipal.FindFirstValue(ClaimTypes.Expiration)));

                    if ((tokenExpiresAt - DateTime.Now).TotalDays < 1.0) {
                        UserExceptionCreator<InvalidIdentityException>.Create(
                            IdentityValidationMessages.TOKEN_EXPIRED,
                            SignInErrorResponseModel.New(SignInErrorResponseType.TokenExpired,
                                IdentityValidationMessages.TOKEN_EXPIRED)).Throw();
                    }

                    currentUser.LastLoggedIn = DateTime.Now;

                    repository.UpdateUserLastLoggedInDate(currentUser.Id, currentUser.LastLoggedIn.Value);

                    return new UserAccount(currentUser) {
                        TokenExpiresAt = tokenExpiresAt,
                    };
                }
            });

        public Task<UserAccount> NewUser(NewUserDataContract newUserDataContract) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository(connection);
                     IIdentityRolesRepository rolesRepository = _identityRepositoriesFactory.NewIdentityRolesRepository(connection);

                     if (!Regex.IsMatch(newUserDataContract.Password, ConfigurationManager.AppSettings.PasswordStrongRegex)) {
                         throw new ArgumentException(ConfigurationManager.AppSettings.PasswordWeakErrorMessage);
                     }

                     if (!Validator.IsEmailValid(newUserDataContract.Email)) {
                         throw new ArgumentException(IdentityValidationMessages.EMAIL_INVALID);
                     }

                     if (!identityRepository.IsEmailAvailable(newUserDataContract.Email)) {
                         throw new ArgumentException(IdentityValidationMessages.EMAIL_NOT_AVAILABLE);
                     }

                     string passwordSalt = CryptoHelper.CreateSalt();

                     string hashedPassword = CryptoHelper.Hash(newUserDataContract.Password, passwordSalt);

                     UserIdentity newUser = new UserIdentity {
                         CanUserResetExpiredPassword = true,
                         Email = newUserDataContract.Email,
                         PasswordExpiresAt =
                             (newUserDataContract.PasswordExpiresAt.Date - DateTime.Now.Date).TotalDays < 0
                                 ? DateTime.Now.Date.AddDays(ConfigurationManager.AppSettings.PasswordExpiryDays)
                                 : newUserDataContract.PasswordExpiresAt,
                         ForceChangePassword = newUserDataContract.ForceChangePassword,
                         PasswordHash = hashedPassword,
                         PasswordSalt = passwordSalt
                     };

                     newUser.IsPasswordExpired = (newUser.PasswordExpiresAt - DateTime.Now.Date).TotalDays < 0;

                     newUser.Id = identityRepository.NewUser(newUser);

                     if (newUserDataContract.Roles.Any()) {
                         rolesRepository.AssignRoles(newUser.Id, newUserDataContract.Roles);
                     }

                     return identityRepository.GetAccountByUserId(newUser.Id);
                 }
             });

        public Task<UserAccount> UpdatePassword(ResetPasswordDataContract authenticateDataContract) =>
           Task.Run(() => {
               if (!Validator.IsEmailValid(authenticateDataContract.Email))
                   throw new ArgumentException(IdentityValidationMessages.EMAIL_INVALID);

               using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                   IIdentityRepository repository = _identityRepositoriesFactory.NewIdentityRepository(connection);
                   UserIdentity user = repository.GetUserByEmail(authenticateDataContract.Email);

                   if (user.IsDeleted) {
                       UserExceptionCreator<InvalidIdentityException>.Create(
                           IdentityValidationMessages.USER_DELETED,
                           SignInErrorResponseModel.New(SignInErrorResponseType.UserDeleted,
                               IdentityValidationMessages.USER_DELETED)).Throw();
                   }

                   if (!CryptoHelper.Validate(authenticateDataContract.Password, user.PasswordSalt, user.PasswordHash)) {
                       UserExceptionCreator<InvalidIdentityException>.Create(
                           IdentityValidationMessages.INVALID_CREDENTIALS,
                           SignInErrorResponseModel.New(SignInErrorResponseType.InvalidCredentials,
                               IdentityValidationMessages.INVALID_CREDENTIALS)).Throw();
                   }

                   if (user.IsPasswordExpired && !user.CanUserResetExpiredPassword) {
                       UserExceptionCreator<InvalidIdentityException>.Create(
                           IdentityValidationMessages.USER_NOT_ALLOW_TO_RESET_PASSWORD,
                           SignInErrorResponseModel.New(SignInErrorResponseType.PasswordExpired,
                               IdentityValidationMessages.USER_NOT_ALLOW_TO_RESET_PASSWORD)).Throw();
                   }

                   if (authenticateDataContract.Password == authenticateDataContract.NewPassword) {
                       UserExceptionCreator<InvalidIdentityException>.Create(
                           IdentityValidationMessages.PASSWORD_MUST_BE_DIFFERENT,
                           SignInErrorResponseModel.New(SignInErrorResponseType.InvalidCredentials,
                               IdentityValidationMessages.PASSWORD_MUST_BE_DIFFERENT)).Throw();
                   }

                   if (!UpdatePassword(user, authenticateDataContract.NewPassword, repository)) {
                       UserExceptionCreator<InvalidIdentityException>.Create(
                           ConfigurationManager.AppSettings.PasswordWeakErrorMessage,
                           SignInErrorResponseModel.New(SignInErrorResponseType.InvalidCredentials,
                               ConfigurationManager.AppSettings.PasswordWeakErrorMessage)).Throw();
                   }

                   if (user.ForceChangePassword) {
                       user.ForceChangePassword = false;

                       repository.UpdateUser(user);
                   }

                   return SignInAsync(new AuthenticationDataContract { Password = authenticateDataContract.NewPassword, Email = authenticateDataContract.Email });
               }
           });

        public Task<UserAccount> UpdateUser(UpdateUserDataContract updateUserDataContract) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository(connection);
                    IIdentityRolesRepository rolesRepository = _identityRepositoriesFactory.NewIdentityRolesRepository(connection);

                    if (updateUserDataContract.Id <= 0) {
                        throw new ArgumentException(IdentityValidationMessages.USER_NOT_SPECIFIED);
                    }

                    UserIdentity userIdentity = identityRepository.GetUserById(updateUserDataContract.Id);

                    if (userIdentity == null) {
                        throw new ArgumentException(IdentityValidationMessages.USER_NOT_EXISTS);
                    }

                    if (!userIdentity.Email.ToLower().Equals(updateUserDataContract.Email.ToLower())) {
                        if (!Validator.IsEmailValid(updateUserDataContract.Email)) {
                            throw new ArgumentException(IdentityValidationMessages.EMAIL_INVALID);
                        } else if (!identityRepository.IsEmailAvailable(updateUserDataContract.Email)) {
                            throw new ArgumentException(IdentityValidationMessages.EMAIL_NOT_AVAILABLE);
                        } 
                    }

                    if (!string.IsNullOrEmpty(updateUserDataContract.Password)) {
                        if (!Regex.IsMatch(updateUserDataContract.Password, ConfigurationManager.AppSettings.PasswordStrongRegex)) {
                            throw new ArgumentException(ConfigurationManager.AppSettings.PasswordWeakErrorMessage);
                        }

                        userIdentity.PasswordSalt = CryptoHelper.CreateSalt();

                        userIdentity.PasswordHash = CryptoHelper.Hash(updateUserDataContract.Password, userIdentity.PasswordSalt);
                    }

                    userIdentity.ForceChangePassword = updateUserDataContract.ForceChangePassword;
                    userIdentity.PasswordExpiresAt =
                        (updateUserDataContract.PasswordExpiresAt.Date - DateTime.Now.Date).TotalDays < 0
                            ? DateTime.Now.Date.AddDays(ConfigurationManager.AppSettings.PasswordExpiryDays)
                            : updateUserDataContract.PasswordExpiresAt;
                    userIdentity.Email = updateUserDataContract.Email;
                    userIdentity.IsPasswordExpired = (userIdentity.PasswordExpiresAt - DateTime.Now.Date).TotalDays < 0;

                    identityRepository.UpdateUser(userIdentity);

                    return identityRepository.GetAccountByUserId(userIdentity.Id);
                }
            });

        public Task<bool> IsEmailAvailable(string email) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository(connection);

                     if (!Validator.IsEmailValid(email)) {
                         throw new ArgumentException(IdentityValidationMessages.EMAIL_INVALID);
                     }

                     return identityRepository.IsEmailAvailable(email);
                 }
             });

        private bool IsUserPasswordExpired(
            UserIdentity user) {
            if (user.IsPasswordExpired) { return true; }

            if (DateTime.UtcNow > user.PasswordExpiresAt) {
                user.IsPasswordExpired = true;
                return true;
            }

            return false;
        }

        private bool UpdatePassword(
            UserIdentity user,
            string newPassword,
            IIdentityRepository repository) {

            if (!Regex.IsMatch(newPassword, ConfigurationManager.AppSettings.PasswordStrongRegex)) {
                //_logger.LogInformation("New password did not match minimum strong password requirements: {0}", ConfigurationManager.AppSettings.PasswordStrongRegex);
                return false;
            }

            if (user.CanUserResetExpiredPassword) {
                user.PasswordExpiresAt = DateTime.UtcNow.AddDays(ConfigurationManager.AppSettings.PasswordExpiryDays);
            }

            string salt = CryptoHelper.CreateSalt();
            user.PasswordSalt = salt;
            user.PasswordHash = CryptoHelper.Hash(newPassword, salt);
            user.IsPasswordExpired = false;

            repository.UpdateUserPassword(user);

            return true;
        }
    }
}