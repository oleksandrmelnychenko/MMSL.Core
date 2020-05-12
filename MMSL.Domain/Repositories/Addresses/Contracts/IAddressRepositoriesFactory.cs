using System.Data;

namespace MMSL.Domain.Repositories.Addresses.Contracts {
    public interface IAddressRepositoriesFactory {
        public IAddressRepository NewAddressRepository(IDbConnection connection);

    }
}
