using MMSL.Domain.DataContracts.FittingTypes;
using MMSL.Domain.Entities.Measurements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IFittingTypeService {
        Task<List<FittingType>> GetFittingTypesAsync(string searchPhrase);
        Task<FittingType> AddFittingTypeAsync(FittingTypeDataContract fittingTypeDataContract);
    }
}
