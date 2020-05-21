using MMSL.Domain.Repositories.Measurements.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementValueRepository : IMeasurementValueRepository {

        private readonly IDbConnection _connection;

        public MeasurementValueRepository(IDbConnection connection) {
            this._connection = connection;
        }


    }
}
