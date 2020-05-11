using System.Linq;
using System.Security.Claims;

namespace MMSL.Common.Helpers {
    public class ClaimHelper {
        public static long GetUserId(ClaimsPrincipal currentUser) {
            Claim claim = currentUser.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            return long.Parse(claim.Value);
        }

        //public static string GetUserRole(ClaimsPrincipal currentUser) {
        //    Claim contactRole = currentUser.Claims.FirstOrDefault(c => c.Type.Equals(@"http://schemas.microsoft.com/ws/2008/06/identity/claims/role"));
        //    return contactRole.Value;
        //}
    }
}
