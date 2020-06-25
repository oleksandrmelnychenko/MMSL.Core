using MMSL.Domain.Entities.Options;

namespace MMSL.Domain.DataContracts.ProductOptions {
    public class UpdateOptionGroupDataContract : EntityDataContractBase<OptionGroup> {
        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public bool IsMandatory { get; set; }

        public decimal? Price { get; set; }

        public long? CurrencyTypeId { get; set; }

        public override OptionGroup GetEntity() {
            return new OptionGroup {
                Id = Id,
                IsDeleted = IsDeleted,
                IsMandatory = IsMandatory,
                Name = Name
            };
        }
    }
}
