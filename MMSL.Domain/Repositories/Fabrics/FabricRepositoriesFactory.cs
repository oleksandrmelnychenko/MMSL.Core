using MMSL.Domain.Repositories.Fabrics.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Fabrics {
    public class FabricRepositoriesFactory : IFabricRepositoriesFactory {
        public IFabricRepository NewFabricRepository(IDbConnection connection) =>
            new FabricRepository(connection);
    }
}
