using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementMapDefinitionRepository : IMeasurementMapDefinitionRepository {


        private readonly IDbConnection _connection;

        public MeasurementMapDefinitionRepository(IDbConnection connection) {
            this._connection = connection;
        }
    }
}
