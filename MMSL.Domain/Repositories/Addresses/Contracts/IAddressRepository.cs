using MMSL.Domain.Entities.Addresses;

namespace MMSL.Domain.Repositories.Addresses.Contracts {
    public interface IAddressRepository {

        Address GetAddress(long addressId);

        long AddAddress(Address address);
        
        void UpdateAddress(Address address);

    }
}
