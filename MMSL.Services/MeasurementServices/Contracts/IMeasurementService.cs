using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementService {
        Task<List<Measurement>> GetMeasurementsAsync(string searchPhrase);
        Task<Measurement> GetMeasurementDetailsAsync(long measurementId);
        Task<Measurement> NewMeasurementAsync(NewMeasurementDataContract newMeasurementDataContract);
        Task UpdateMeasurementAsync(Measurement measurement);
        Task DeleteMeasurementAsync(long measurementId);
    }
}
