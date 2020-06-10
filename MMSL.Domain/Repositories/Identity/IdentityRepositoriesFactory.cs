using System.Data;
using MMSL.Domain.Repositories.Identity.Contracts;

namespace MMSL.Domain.Repositories.Identity
{
    public class IdentityRepositoriesFactory : IIdentityRepositoriesFactory
    {
        public IIdentityRepository NewIdentityRepository(IDbConnection connection) =>
            new IdentityRepository(connection);

        public IIdentityRolesRepository NewIdentityRolesRepository(IDbConnection connection) =>
            new IdentityRolesRepository(connection);

    }
}
