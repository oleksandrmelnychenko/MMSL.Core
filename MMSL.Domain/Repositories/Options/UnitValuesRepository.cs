using MMSL.Domain.Repositories.Options.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Options {
    class UnitValuesRepository : IUnitValuesRepository {

        private readonly IDbConnection _connection;

        public UnitValuesRepository(IDbConnection connection) {
            _connection = connection;
        }


    }
}
