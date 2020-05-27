﻿using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementsRepositoriesFactory : IMeasurementsRepositoriesFactory {
        public IMeasurementRepository NewMeasurementRepository(IDbConnection connection) =>
            new MeasurementRepository(connection);

        public IMeasurementSizeRepository NewMeasurementSizeRepository(IDbConnection connection) =>
            new MeasurementSizeRepository(connection);

        public IMeasurementDefinitionRepository NewMeasurementDefinitionRepository(IDbConnection connection) =>
            new MeasurementDefinitionRepository(connection);

        public IMeasurementMapDefinitionRepository NewMeasurementMapDefinitionRepository(IDbConnection connection) =>
            new MeasurementMapDefinitionRepository(connection);

        public IMeasurementMapValueRepository NewMeasurementValueRepository(IDbConnection connection) =>
            new MeasurementMapValueRepository(connection);
    }
}
