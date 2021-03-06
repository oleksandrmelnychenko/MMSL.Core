﻿using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices.Contracts {
    public interface IMeasurementSizeService {
        Task<List<MeasurementSize>> GetMeasurementSizesAsync(long measurementId);
        Task<MeasurementSize> GetMeasurementSizeByIdAsync(long measurementSizeId);
        Task<MeasurementSize> AddMeasurementSizeAsync(MeasurementSizeDataContract measurementSizeDataContract);
        Task<MeasurementSize> UpdateMeasurementSizeAsync(UpdateMeasuremetSizeDataContract measurementSize);
        Task<MeasurementSize> DeleteMeasurementSizeAsync(long measurementId, long measurementSizeId);
    }
}
