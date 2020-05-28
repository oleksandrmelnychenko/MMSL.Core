using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementRepository {
        List<Measurement> GetAll(string searchPhrase);
        List<Measurement> GetAllByProduct(long productCategoryId);
        Measurement NewMeasurement(NewMeasurementDataContract newMeasurementDataContract);
        void UpdateMeasurement(Measurement measurement);
        Measurement GetById(long measurementId);
        Measurement GetByIdWithDefinitions(long measurementId);
        Measurement GetByIdWithDependencies(long measurementId);
        List<MeasurementMapSize> GetSizesByMeasurementId(long measurementId, long? parentMeasurementId);
    }
}
