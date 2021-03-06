﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementsRepositoriesFactory {

        IMeasurementRepository NewMeasurementRepository(IDbConnection connection);

        IMeasurementSizeRepository NewMeasurementSizeRepository(IDbConnection connection);

        IMeasurementDefinitionRepository NewMeasurementDefinitionRepository(IDbConnection connection);

        IMeasurementMapDefinitionRepository NewMeasurementMapDefinitionRepository(IDbConnection connection);
        
        IMeasurementMapValueRepository NewMeasurementMapValueRepository(IDbConnection connection);

        IMeasurementMapSizeRepository NewMeasurementMapSizeRepository(IDbConnection connection);

        IFittingTypeRepository NewFittingTypeRepository(IDbConnection connection);

        IMeasurementUnitsRepository NewMeasurementUnitsRepository(IDbConnection connection);
    }
}
