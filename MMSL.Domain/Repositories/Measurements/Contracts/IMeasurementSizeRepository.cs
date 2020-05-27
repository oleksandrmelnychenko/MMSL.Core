using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementSizeRepository {
        List<MeasurementSize> GetAllByMeasurementId(long measurementId);
        MeasurementSize GetById(long measurementSizeId);
        MeasurementSize AddMeasurementSize(string name, string description);
        void UpdateMeasurementSize(MeasurementSize measurementSize);
    }
}
