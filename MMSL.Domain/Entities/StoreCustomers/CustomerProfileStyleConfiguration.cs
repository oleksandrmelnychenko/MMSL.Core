using MMSL.Domain.Entities.Options;

namespace MMSL.Domain.Entities.StoreCustomers {
    public class CustomerProfileStyleConfiguration : EntityBase {
        public long OptionUnitId { get; set; }
        public OptionUnit OptionUnit { get; set; }

        public long? UnitValueId { get; set; }
        public UnitValue UnitValue { get; set; }

        public long CustomerProductProfileId { get; set; }
        public CustomerProductProfile CustomerProductProfile { get; set; }
    }
}
