using MMSL.Domain.Entities.Products;
using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface ICustomerProductProfileRepository {
        List<ProductCategory> GetProductsWithProfiles(long dealerIdentityId, long storeCustomerId);
        CustomerProductProfile GetCustomerProductProfile(long profileId);
        List<CustomerProductProfile> GetCustomerProductProfilesByDealerIdentity(long dealerIdentityId, long? productCategoryId);
        long AddCustomerProductProfile(CustomerProductProfile profile);
        CustomerProductProfile UpdateCustomerProductProfile(CustomerProductProfile entity);
    }
}
