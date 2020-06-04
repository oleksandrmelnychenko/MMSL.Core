using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementRepository {
        List<Measurement> GetAll(string searchPhrase);
        List<Measurement> GetAllByProduct(long productCategoryId);
        Measurement NewMeasurement(NewMeasurementDataContract newMeasurementDataContract);
        void UpdateMeasurement(Measurement measurement);
        Measurement GetById(long measurementId);
        Measurement GetByIdWithDefinitions(long measurementId);
        List<MeasurementMapSize> GetSizesByMeasurementId(long measurementId, long? parentMeasurementId);
    }
}
