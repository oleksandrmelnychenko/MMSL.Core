using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementValueRepository {

        long AddValue(MeasurementValue value);
        MeasurementValue UpdateValue(MeasurementValue measurementValue);
    }
}
