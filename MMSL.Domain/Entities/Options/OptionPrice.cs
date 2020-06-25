using MMSL.Domain.Entities.CurrencyTypes;

namespace MMSL.Domain.Entities.Options {
    public class OptionPrice : EntityBase {
        public decimal Price { get; set; }

        public long CurrencyTypeId { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public long? OptionGroupId { get; set; }
        public OptionGroup OptionGroup { get; set; }

        public long? OptionUnitId { get; set; }
        public OptionUnit OptionUnit { get; set; }
    }
}
