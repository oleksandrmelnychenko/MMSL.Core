using MMSL.Domain.Entities.CurrencyTypes;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Types.Contracts {
    public interface ICurrencyTypeRepository {
        List<CurrencyType> GetCurrencyTypes();
        CurrencyType GetCurrencyType(long id);
        long AddCurrencyType(CurrencyType currencyType);
        CurrencyType UpdateCurrencyType(CurrencyType currencyType);
    }
}
