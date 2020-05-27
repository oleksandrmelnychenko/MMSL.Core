namespace MMSL.Domain.DataContracts {
    public class NewMeasurementDataContract {
        public long ProductCategoryId { get; set; }

        public long BaseMeasurementId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class NewBaseMeasurementDataContract {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
