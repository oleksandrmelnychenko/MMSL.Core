using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.StoreCustomerServices.Contracts {
    public interface IStoreCustomerService {
        Task<List<StoreCustomer>> GetCustomersByStoreAsync(long storeId);
        Task<StoreCustomer> GetCustomerAsync(long storeCustomerId);
        Task<StoreCustomer> AddCustomerAsync(StoreCustomer storeCustomer);
        Task<StoreCustomer> UpdateCustomerAsync(StoreCustomer storeCustomer);
        Task<StoreCustomer> DeleteCustomerAsync(long storeCustomerId);

    }
}
