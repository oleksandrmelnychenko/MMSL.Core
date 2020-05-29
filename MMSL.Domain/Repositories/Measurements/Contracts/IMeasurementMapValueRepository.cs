using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementMapValueRepository {
        MeasurementMapValue GetValue(long id);
        long AddValue(MeasurementMapValue value);
        MeasurementMapValue UpdateValue(MeasurementMapValue measurementValue);
    }
}
