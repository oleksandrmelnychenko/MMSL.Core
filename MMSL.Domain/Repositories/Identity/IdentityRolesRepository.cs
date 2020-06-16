using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.Repositories.Identity.Contracts;

namespace MMSL.Domain.Repositories.Identity {
    public class IdentityRolesRepository : IIdentityRolesRepository {
        private readonly IDbConnection _connection;

        public IdentityRolesRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<UserRole> AssignRoles(long userId, List<RoleType> roles) {
            List<UserRole> userRolesToReturn = new List<UserRole>();

            foreach (RoleType role in roles) {
                UserRole userRole = _connection.Query<UserRole>(
                    "INSERT INTO [UserRoles] (IsDeleted, UserIdentityId, UserRoleTypeId) " +
                    "VALUES(0,@UserIdentityId," +
                    "(SELECT TOP (1) [Id] FROM [UserIdentityRoleTypes] WHERE RoleType = @Role)" +
                    ") " +
                    "SELECT * FROM [UserRoles] WHERE ID = (SELECT SCOPE_IDENTITY()) "
                    , new { UserIdentityId = userId, Role = role }
                ).SingleOrDefault();

                userRolesToReturn.Add(userRole);
            }

            return userRolesToReturn;
        }

    }
}
