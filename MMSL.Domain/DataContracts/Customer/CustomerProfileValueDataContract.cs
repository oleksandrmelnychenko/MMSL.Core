namespace MMSL.Domain.DataContracts.Customer {
    public class NewCustomerProfileValueDataContract {
        /// <summary>
        ///     Body measurement
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        ///     Add to body measurement
        /// </summary>
        public float? FittingValue { get; set; }

        public long MeasurementDefinitionId { get; set; }

    }
}
