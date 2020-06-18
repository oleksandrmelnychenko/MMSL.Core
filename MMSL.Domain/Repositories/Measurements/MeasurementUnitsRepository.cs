using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Measurements {
    class MeasurementUnitsRepository : IMeasurementUnitsRepository {

        private readonly IDbConnection _connection;

        public MeasurementUnitsRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<MeasurementUnit> GetAll() => 
            _connection.Query<MeasurementUnit>(@"SELECT * FROM [MeasurementUnits]").ToList();
    }
}
