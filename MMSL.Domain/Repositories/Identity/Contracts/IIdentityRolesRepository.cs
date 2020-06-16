using MMSL.Domain.Entities.Identity;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Identity.Contracts {
    public interface IIdentityRolesRepository {
        List<UserRole> AssignRoles(long userId, List<RoleType> roles);
    }
}
