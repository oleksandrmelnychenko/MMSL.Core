using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface IStoreCustomerRepository {
        List<StoreCustomer> GetStoreCustomers(long storeId);
        StoreCustomer GetStoreCustomer(long storeCustomerId);
        long AddStoreCustomer(StoreCustomer storeCustomer);
        void UpdateStoreCustomer(StoreCustomer storeCustomer);
    }
}
