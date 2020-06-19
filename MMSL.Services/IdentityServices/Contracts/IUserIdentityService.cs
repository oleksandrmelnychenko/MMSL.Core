using System.Security.Claims;
using System.Threading.Tasks;
using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.Identity;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Services.IdentityServices.Contracts {

    public interface IUserIdentityService {

        Task<UserAccount> SignInAsync(AuthenticationDataContract authenticateDataContract);

        Task<UserAccount> ValidateToken(ClaimsPrincipal userPrincipal);

        Task<UserAccount> NewUser(NewUserDataContract newUserDataContract);

        Task<UserAccount> UpdateUser(UpdateUserDataContract updateUserDataContract);

        Task<UserAccount> UpdatePassword(ResetPasswordDataContract authenticateDataContract);

        Task<bool> IsEmailAvailable(string email);
    }
}