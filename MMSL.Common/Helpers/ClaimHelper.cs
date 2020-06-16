using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MMSL.Common.Helpers {
    public class ClaimHelper {
        public static long GetUserId(ClaimsPrincipal currentUser) {
            Claim claim = currentUser.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            return long.Parse(claim.Value);
        }

        public static string[] GetUserRoles(ClaimsPrincipal currentUser) {
            return currentUser.Claims
                .Where(c => c.Type.Equals(ClaimTypes.Role))
                .Select(x => x.Value)
                .ToArray();
        }
    }
}
