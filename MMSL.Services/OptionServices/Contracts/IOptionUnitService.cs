using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.ProductOptions;
using MMSL.Domain.Entities.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices.Contracts {
    public interface IOptionUnitService {
        Task<List<OptionUnit>> GetOptionUnitsByGroupIdAsync(long optionGroupId);
        Task<OptionUnit> GetOptionUnitByIdAsync(long optionUnitId);
        Task<OptionUnit> AddOptionUnit(OptionUnit optionUnit, List<UnitValueDataContract> values);
        Task<OptionUnit> UpdateOptionUnit(OptionUnit optionUnit, List<UnitValueDataContract> values);
        Task<OptionUnit> DeleteOptionUnit(long optionUnitId);
        Task UpdateOrderIndexesAsync(List<UpdateOrderIndexDataContract> optionIndexes);
    }
}
