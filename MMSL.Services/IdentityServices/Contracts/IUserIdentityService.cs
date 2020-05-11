using System.Security.Claims;
using System.Threading.Tasks;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Services.IdentityServices.Contracts {

    public interface IUserIdentityService {

        Task<UserAccount> SignInAsync(AuthenticationDataContract authenticateDataContract);


        Task<UserAccount> ValidateToken(ClaimsPrincipal userPrincipal);

        Task<UserAccount> NewUser(NewUserDataContract newUserDataContract);
    }
}