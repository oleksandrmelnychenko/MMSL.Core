using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface ICustomerProfileSizeValueRepository {
        CustomerProfileSizeValue GetSizeValue(long sizeValueId);
        long AddSizeValue(CustomerProfileSizeValue sizeValue);
        void UpdateSizeValue(CustomerProfileSizeValue sizeValue);
    }
}
