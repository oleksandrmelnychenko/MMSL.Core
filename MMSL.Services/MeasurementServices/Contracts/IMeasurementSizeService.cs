using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementSizeService {
        Task<List<MeasurementSize>> GetMeasurementSizes(long measurementId);
    }
}
