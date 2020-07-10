namespace MMSL.Domain.DataContracts.Filters {
    public sealed class FabricFilterValue {
        public string Value { get; private set; }
        public bool Applied { get; set; }

        public FabricFilterValue(string value) {
            Value = value;
        }
    }
}
