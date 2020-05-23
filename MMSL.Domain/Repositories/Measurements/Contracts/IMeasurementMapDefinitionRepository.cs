using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementMapDefinitionRepository {
        MeasurementMapDefinition GetMeasurementMapDefinition(long measurementMapDefinitionId);
        long AddMeasurementMapDefinition(MeasurementMapDefinition measurementMapDefinition);
        MeasurementMapDefinition UpdateMeasurementMapDefinition(MeasurementMapDefinition measurementMapDefinition);
    }
}
