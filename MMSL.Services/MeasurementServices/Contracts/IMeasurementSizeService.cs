using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementSizeService {
        Task<List<MeasurementSize>> GetMeasurementSizesAsync(long measurementId);
        Task<MeasurementSize> AddMeasurementSizeAsync(MeasurementSizeDataContract measurementSizeDataContract);
        Task<MeasurementSize> UpdateMeasurementSizeAsync(MeasurementSize measurementSize);
        Task<MeasurementSize> DeleteMeasurementSizeAsync(long measurementSizeId);
    }
}
