﻿using MMSL.Domain.Repositories.Measurements.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementDefinitionRepository : IMeasurementDefinitionRepository {


        private readonly IDbConnection _connection;

        public MeasurementDefinitionRepository(IDbConnection connection) {
            this._connection = connection;
        }
    }
}
