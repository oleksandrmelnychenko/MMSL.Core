using System.Data;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface IStoreRepositoriesFactory {
        IStoreRepository NewStoreRepository(IDbConnection connection);

        IStoreCustomerRepository NewStoreCustomerRepository(IDbConnection connection);

        ICustomerProductProfileRepository NewCustomerProductProfileRepository(IDbConnection connection);

        ICustomerProfileSizeValueRepository NewCustomerProfileSizeValueRepository(IDbConnection connection);

        ICustomerProfileStyleConfigurationRepository NewCustomerProfileStyleConfigurationRepository(IDbConnection connection);
    }
}
