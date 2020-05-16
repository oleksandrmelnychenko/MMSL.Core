using MMSL.Domain.Entities.PaymentTypes;

namespace MMSL.Domain.DataContracts.Types {
    public class PaymentTypeDataContract : NamedEntityDataContractBase<PaymentType> {
        public override PaymentType GetEntity() {
            return new PaymentType {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }
    }
}
