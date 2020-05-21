using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementMapDefinitionRepository {
        MeasurementMapDefinition GetMeasurementMapDefinition(MeasurementMapDefinition measurementMapDefinition);
        long AddMeasurementMapDefinition(MeasurementMapDefinition measurementMapDefinition);
        MeasurementMapDefinition UpdateMeasurementMapDefinition(MeasurementMapDefinition measurementMapDefinition);
    }
}
