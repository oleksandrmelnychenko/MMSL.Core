using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementSizeService {
        Task<List<MeasurementSize>> GetMeasurementSizes(long measurementId);
        Task<MeasurementSize> AddMeasurementSize(MeasurementSize measurementSize);
        Task<MeasurementSize> UpdateMeasurementSize(MeasurementSize measurementSize);
        Task<MeasurementSize> DeleteMeasurementSize(MeasurementSize measurementSize);
    }
}
