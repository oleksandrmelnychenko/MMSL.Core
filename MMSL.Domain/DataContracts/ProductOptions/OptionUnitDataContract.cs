using MMSL.Domain.Entities.Options;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.ProductOptions {
    public class OptionUnitDataContract : EntityDataContractBase<OptionUnit> {
        public int OrderIndex { get; set; }

        public string Value { get; set; }

        public bool IsMandatory { get; set; }

        public long OptionGroupId { get; set; }

        public string SerializedValues { get; set; }

        public decimal? Price { get; set; }

        public long? CurrencyTypeId { get; set; }

        public override OptionUnit GetEntity() {
            return new OptionUnit {
                Id = Id,
                OrderIndex = OrderIndex,
                IsMandatory = IsMandatory,
                Value = Value,
                OptionGroupId = OptionGroupId
            };
        }
    }
}
