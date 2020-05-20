using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementService {
        Task<List<Measurement>> GetMeasurementsAsync(string searchPhrase);
        Task<Measurement> NewMeasurementAsync(NewMeasurementDataContract newMeasurementDataContract);
    }
}
