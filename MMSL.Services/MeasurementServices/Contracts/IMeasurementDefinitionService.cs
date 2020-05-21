using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementDefinitionService {
        Task<List<MeasurementDefinition>> GetMeasurementDefinitionsAsync(string searchPhrase, bool? isDefault);
        Task<MeasurementDefinition> NewMeasurementDefinitionAsync(NewMeasurementDefinitionDataContract newMeasurementDefinitionDataContract);
        Task UpdateMeasurementDefinitionAsync(MeasurementDefinition measurementDefinition);
        Task DeleteMeasurementDefinitionAsync(long measurementDefinitionId);
    }
}
