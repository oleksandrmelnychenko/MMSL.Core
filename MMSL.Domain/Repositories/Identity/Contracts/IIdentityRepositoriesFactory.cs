using System.Data;

namespace MMSL.Domain.Repositories.Identity.Contracts
{
    public interface IIdentityRepositoriesFactory
    {
        IIdentityRepository NewIdentityRepository(IDbConnection connection);

        IIdentityRolesRepository NewIdentityRolesRepository(IDbConnection connection);
    }
}
