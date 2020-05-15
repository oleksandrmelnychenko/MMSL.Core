using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.EntityHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.StoreCustomerServices.Contracts {
    public interface IStoreCustomerService {
        Task<PaginatingResult<StoreCustomer>> GetCustomersByStoreAsync(long? storeId, int pageNumber, int limit, string searchPhrase);
        Task<StoreCustomer> GetCustomerAsync(long storeCustomerId);
        Task<StoreCustomer> AddCustomerAsync(StoreCustomer storeCustomer);
        Task<StoreCustomer> UpdateCustomerAsync(StoreCustomer storeCustomer);
        Task<StoreCustomer> DeleteCustomerAsync(long storeCustomerId);

    }
}
