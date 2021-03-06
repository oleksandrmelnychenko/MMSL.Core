﻿using MMSL.Domain.DataContracts.FittingTypes;
using MMSL.Domain.Entities.Measurements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IFittingTypeService {
        Task<List<FittingType>> GetFittingTypesAsync(string searchPhrase, long measurementId);
        Task<FittingType> AddFittingTypeAsync(FittingTypeDataContract fittingTypeDataContract);
        Task<FittingType> GetFittingTypeByIdAsync(long fittingTypeId);
        Task<FittingType> UpdateFittingTypeAsync(FittingType fittingType);
        Task DeleteFittingTypeAsync(long fittingTypeId);
    }
}
