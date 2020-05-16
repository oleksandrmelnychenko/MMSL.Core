using MMSL.Domain.DataContracts.Types;
using MMSL.Domain.Entities.CurrencyTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.Types.Contracts {
    public interface ICurrencyTypeService {
        Task<List<CurrencyType>> GetCurrencyTypesAsync();
        Task<CurrencyType> AddCurrencyTypeAsync(CurrencyTypeDataContract currencyType);
        Task<CurrencyType> UpdateCurrencyTypeAsync(CurrencyTypeDataContract currencyType);
        Task<CurrencyType> DeleteCurrencyTypeAsync(long currencyTypeId);
    }
}
