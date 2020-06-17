using MMSL.Domain.Entities.Measurements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IFittingTypeRepository {
        List<FittingType> GetAll(string searchPhrase, long mesurementId);
        FittingType Add(string type, string unit, long dealerAccountId, long measurementId);
        FittingType GetById(long fittingTypeId);
        void Update(FittingType fittingType);
    }
}
