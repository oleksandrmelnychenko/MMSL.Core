using MMSL.Domain.Entities.Measurements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IFittingTypeRepository {
        List<FittingType> GetAll(string searchPhrase);
        FittingType Add(string type, string unit, long dealerAccountId);
    }
}
