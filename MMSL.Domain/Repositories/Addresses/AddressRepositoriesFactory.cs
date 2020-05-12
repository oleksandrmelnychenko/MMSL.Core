using MMSL.Domain.Repositories.Addresses.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Addresses {
    public class AddressRepositoriesFactory : IAddressRepositoriesFactory {
        public IAddressRepository NewAddressRepository(IDbConnection connection) =>
            new AddressRepository(connection);
    }
}
