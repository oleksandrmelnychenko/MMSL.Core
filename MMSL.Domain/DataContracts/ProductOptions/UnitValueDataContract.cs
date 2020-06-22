namespace MMSL.Domain.DataContracts.ProductOptions {
    public class UnitValueDataContract {
        public long Id { get; set; }
        public float Value { get; set; }
        public bool IsDeleted { get; set; }
        public int OrderIndex { get; set; }
    }
}
