using System.Data;

namespace MMSL.Domain.Repositories.Identity.Contracts
{
    public interface IIdentityRepositoriesFactory
    {
        IIdentityRepository NewIdentityRepository(IDbConnection connection);

        IIdentityRolesRepository NewIdentityRolesRepository(IDbConnection connection);

        IAccountTypeCompanyInfoRepository NewAccountTypeCompanyInfoRepository(IDbConnection connection);

        IAccountTypeRepository NewAccountTypeRepository(IDbConnection connection);
    }
}
