using System.Data;

namespace MMSL.Domain.Repositories.Fabrics.Contracts {
    public interface IFabricRepositoriesFactory {
        IFabricRepository NewFabricRepository(IDbConnection connection);
    }
}
