using MMSL.Domain.Entities.Options;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Options.Contracts {
    public interface IOptionPriceRepository {
        OptionPrice GetPrice(long priceId);
        List<OptionPrice> GetPrices(long? optionUnitId, long? optionGroupId);
        OptionPrice AddPrice(OptionPrice price);
        OptionPrice UpdatePrice(OptionPrice price);
    }
}
