using Microsoft.AspNetCore.Http;
using MMSL.Domain.Entities.Options;

namespace MMSL.Domain.DataContracts {
    public class OptionUnitDataContract : EntityDataContractBase<OptionUnit> {
        public int OrderIndex { get; set; }

        public string Value { get; set; }

        public bool IsMandatory { get; set; }

        public long OptionGroupId { get; set; }

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
