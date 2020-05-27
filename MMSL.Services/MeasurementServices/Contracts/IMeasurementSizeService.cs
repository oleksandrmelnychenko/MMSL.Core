using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementSizeService {
        Task<List<MeasurementSize>> GetMeasurementSizes(long measurementId);
        Task<MeasurementSize> AddMeasurementSize(MeasurementSizeDataContract measurementSizeDataContract);
        Task<MeasurementSize> UpdateMeasurementSize(MeasurementSize measurementSize);
        Task<MeasurementSize> DeleteMeasurementSize(long measurementSizeId);
    }
}
