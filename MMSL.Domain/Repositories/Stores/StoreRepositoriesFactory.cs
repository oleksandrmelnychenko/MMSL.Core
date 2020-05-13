using MMSL.Domain.Repositories.Stores.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Stores {
    public class StoreRepositoriesFactory : IStoreRepositoriesFactory {

        public IStoreRepository NewStoreRepository(IDbConnection connection)
            => new StoreRepository(connection);

        public IStoreCustomerRepository NewStoreCustomerRepository(IDbConnection connection)
            => new StoreCustomerRepository(connection);
    }
}
