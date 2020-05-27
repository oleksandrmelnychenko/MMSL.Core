using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    //TODO: update this
    public interface IMeasurementMapValueRepository {

        long AddValue(MeasurementMapValue value);
        MeasurementMapValue UpdateValue(MeasurementMapValue measurementValue);
    }
}
