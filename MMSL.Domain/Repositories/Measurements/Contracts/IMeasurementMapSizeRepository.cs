using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementMapSizeRepository {
        MeasurementMapSize New(long measurementId, long measurementSizeId);
        void Update(MeasurementMapSize measurementMapSize);
    }
}
