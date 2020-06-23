using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MMSL.Domain.Entities.Identity
{
    public class UserAccount : EntityBase
    {
     
        public UserAccount()
        {
        }
      
        public UserAccount(UserIdentity user)
        {
            Id = user.Id;
            Email = user.Email;
            IsPasswordExpired = user.IsPasswordExpired;
            PasswordExpiresAt = user.PasswordExpiresAt;
            LastLoggedIn = user.LastLoggedIn;
            CanUserResetExpiredPassword = user.CanUserResetExpiredPassword;
            ForceChangePassword = user.ForceChangePassword;

            if (user.UserRoles != null) {
                Roles = user.UserRoles.Select(x => x.UserRoleType).ToList();
            }
        }

        /// <summary>
        /// User email address. This address will be used for authenticating the user.
        /// </summary>
        public string Email { get; set; }

        public bool CanUserResetExpiredPassword { get; set; }

        public bool ForceChangePassword { get; set; }

        /// <summary>
        /// Is TRUE if the password has expired. If the password is expired, a token will not be returned.
        /// </summary>
        //[JsonProperty("is-password-expired")]
        public bool IsPasswordExpired { get; set; }

        /// <summary>
        /// Last user log in date and time.
        /// </summary>
        public DateTime? LastLoggedIn { get; set; }

        /// <summary>
        /// The date and time that the password will expire.
        /// </summary>
        //[JsonProperty("password-expires-at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PasswordExpiresAt { get; set; }

        /// <summary>
        /// The number of Ticks representing the date and time that the password will expire.
        /// </summary>
       // [JsonProperty("password-expires-at-ticks", NullValueHandling = NullValueHandling.Ignore)]
        public string PasswordExpiresAtTicks => PasswordExpiresAt?.Ticks.ToString();

        public string Token { get; set; }

        public DateTime? TokenExpiresAt { get; set; }

        public string TokenExpiresAtTicks => TokenExpiresAt?.Ticks.ToString();

        public List<UserIdentityRoleType> Roles { get; set; } = new List<UserIdentityRoleType>();
    }
}
