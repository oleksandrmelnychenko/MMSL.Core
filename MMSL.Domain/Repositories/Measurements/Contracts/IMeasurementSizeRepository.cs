using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementSizeRepository {
        List<MeasurementSize> GetSizesByMeasurementId(long measurementId);
        MeasurementSize GetMeasurementSize(long measurementSizeId);
        long AddMeasurementSize(MeasurementSize measurementSize);
        MeasurementSize UpdateMeasurementSize(MeasurementSize measurementSize);
    }
}
