using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.EntityHelpers;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface IStoreCustomerRepository {
        PaginatingResult<StoreCustomer> GetStoreCustomers(long storeId, int pageNumber, int limit, string searchPhrase);
        StoreCustomer GetStoreCustomer(long storeCustomerId);
        long AddStoreCustomer(StoreCustomer storeCustomer);
        void UpdateStoreCustomer(StoreCustomer storeCustomer);
    }
}
