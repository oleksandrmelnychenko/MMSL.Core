using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IFittingTypeRepository {
        List<FittingType> GetAll(string searchPhrase, long measurementId);
        FittingType Add(string type, long measurementUnitId, long measurementId);
        FittingType GetById(long fittingTypeId);
        void Update(FittingType fittingType);
    }
}
