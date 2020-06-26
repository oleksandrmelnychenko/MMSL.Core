using MMSL.Domain.DataContracts.Customer;
using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.StoreCustomerServices.Contracts {
    public interface IStoreCustomerProductProfileService {
        Task<List<CustomerProductProfile>> GetAllAsync(long dealerIdentityId, long? productId, string searchPhrase = null);
        Task<CustomerProductProfile> GetByIdAsync(long profileId);
        Task<CustomerProductProfile> AddAsync(long dealerIdentityId, NewCustomerProductProfile newProfileDataContract);
        Task<CustomerProductProfile> UpdateAsync(UpdateCustomerProductProfile profileDataContract);
        Task<CustomerProductProfile> DeleteAsync(long customerProductProfileId);
    }
}
