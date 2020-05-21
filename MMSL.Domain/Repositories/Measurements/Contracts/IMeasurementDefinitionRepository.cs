using MMSL.Domain.Entities.Measurements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementDefinitionRepository {
        List<MeasurementDefinition> GetAll(string searchPhrase, bool? isDefault);
       
    }
}
