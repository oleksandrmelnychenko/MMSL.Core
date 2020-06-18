using Microsoft.AspNetCore.Http;
using MMSL.Domain.Entities.Options;

namespace MMSL.Domain.DataContracts.ProductOptions {
    public class OptionUnitUpdateDataContract : EntityDataContractBase<OptionUnit> {

        public int OrderIndex { get; set; }

        public string Value { get; set; }

        public bool IsMandatory { get; set; }

        public string ImageUrl { get; set; }

        public override OptionUnit GetEntity() {
            return new OptionUnit {
                Id = Id,
                OrderIndex = OrderIndex,
                IsMandatory = IsMandatory,
                Value = Value,
                ImageUrl = ImageUrl
            };
        }

    }
}
