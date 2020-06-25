namespace MMSL.Domain.Entities.Options {
    public class UnitValue : EntityBase {
        public string Value { get; set; }

        public long OptionUnitId { get; set; }

        public int OrderIndex { get; set; }

        public OptionUnit OptionUnit { get; set; }
    }
}
