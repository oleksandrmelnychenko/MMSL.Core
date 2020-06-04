namespace MMSL.Domain.DataContracts.Measurements {
    public class NewMeasurementDefinitionDataContract {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }
    }
}
