using MMSL.Domain.Repositories.Stores.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Stores {
    public class StoreRepositoriesFactory : IStoreRepositoriesFactory {

        public IStoreRepository NewStoreRepository(IDbConnection connection)
            => new StoreRepository(connection);

        public IStoreCustomerRepository NewStoreCustomerRepository(IDbConnection connection)
            => new StoreCustomerRepository(connection);

        public ICustomerProductProfileRepository NewCustomerProductProfileRepository(IDbConnection connection)
            => new CustomerProductProfileRepository(connection);

        public ICustomerProfileSizeValueRepository NewCustomerProfileSizeValueRepository(IDbConnection connection)
            => new CustomerProfileSizeValueRepository(connection);
    }
}
