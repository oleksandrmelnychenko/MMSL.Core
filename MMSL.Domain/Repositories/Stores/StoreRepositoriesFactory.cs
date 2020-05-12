using MMSL.Domain.Repositories.Stores.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Stores {
    public class StoreRepositoriesFactory : IStoreRepositoriesFactory {
        public IStoreRepository NewBankDetailRepository(IDbConnection connection)
            => new StoreRepository(connection);
    }
}
