using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementDefinitionRepository {
        List<MeasurementDefinition> GetAll(string searchPhrase, bool? isDefault);

        MeasurementDefinition NewMeasurementDefinition(MeasurementDefinition newMeasurementDefinitionDataContract);

        void UpdateMeasurementDefinition(MeasurementDefinition measurementDefinition);

        MeasurementDefinition GetById(long measurementDefinitionId);

        int CountDefinitionReferences(long measurementDefinitionId);
    }
}
