using MMSL.Domain.Repositories.Fabrics.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Fabrics {
    class FabricRepository : IFabricRepository {
        private readonly IDbConnection _connection;

        public FabricRepository(IDbConnection connection) {
            this._connection = connection;
        }
    }
}
