using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface ICustomerProfileStyleConfigurationRepository {
        CustomerProfileStyleConfiguration GetById(long id); 
        long Add(CustomerProfileStyleConfiguration styleConfiguration);
        CustomerProfileStyleConfiguration Update(CustomerProfileStyleConfiguration styleConfiguration);
    }
}
