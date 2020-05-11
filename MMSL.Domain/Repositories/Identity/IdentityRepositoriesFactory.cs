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

        public IAccountTypeCompanyInfoRepository NewAccountTypeCompanyInfoRepository(IDbConnection connection) =>
            new AccountTypeCompanyInfoRepository(connection);

        public IAccountTypeRepository NewAccountTypeRepository(IDbConnection connection) =>
            new AccountTypeRepository(connection);

    }
}
