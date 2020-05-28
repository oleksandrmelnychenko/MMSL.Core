using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementService {
        Task<List<Measurement>> GetMeasurementsAsync(string searchPhrase);
        Task<List<Measurement>> GetProductMeasurementsAsync(long productCategoryId);
        Task<Measurement> GetMeasurementDetailsAsync(long measurementId);
        Task<Measurement> GetMeasurementChartAsync(long measurementId);
        Task<Measurement> NewMeasurementAsync(NewMeasurementDataContract newMeasurementDataContract);
        Task UpdateMeasurementAsync(UpdateMeasurementDataContract measurement);
        Task DeleteMeasurementAsync(long measurementId);
    }
}
