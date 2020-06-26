using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.Entities.StoreCustomers {
    public class CustomerProfileSizeValue : EntityBase {
        /// <summary>
        ///     Body measurement
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        ///     Add to body measurement
        /// </summary>
        public float? FittingValue { get; set; }

        public long MeasurementDefinitionId { get; set; }
        public MeasurementDefinition MeasurementDefinition { get; set; }

        public long CustomerProductProfileId { get; set; }
        public CustomerProductProfile CustomerProductProfile { get; set; }
    }
}
