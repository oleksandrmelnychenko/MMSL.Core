using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementDefinitionService {
        Task<List<MeasurementDefinition>> GetMeasurementDefinitionsAsync(string searchPhrase, bool? isDefault);
        
    }
}
