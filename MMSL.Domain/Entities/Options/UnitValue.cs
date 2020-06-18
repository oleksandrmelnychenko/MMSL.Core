namespace MMSL.Domain.Entities.Options {
    public class UnitValue : EntityBase {
        public float Value { get; set; }

        public long OptionUnitId { get; set; }

        public OptionUnit OptionUnit { get; set; }
    }
}
