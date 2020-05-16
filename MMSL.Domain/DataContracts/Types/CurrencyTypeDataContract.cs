using MMSL.Domain.Entities.CurrencyTypes;

namespace MMSL.Domain.DataContracts.Types {
    public class CurrencyTypeDataContract : NamedEntityDataContractBase<CurrencyType> {
        public override CurrencyType GetEntity() {
            return new CurrencyType {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }
    }
}
