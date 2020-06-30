using MMSL.Common.Helpers;
using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Domain.DataContracts.Customer {
    public class UpdateCustomerProfileValueDataContract : EntityDataContractBase<CustomerProfileSizeValue> {
        /// <summary>
        ///     Body measurement
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     Add to body measurement
        /// </summary>
        public string FittingValue { get; set; }

        public long MeasurementDefinitionId { get; set; }

        public override CustomerProfileSizeValue GetEntity() {
            float value = 0;
            bool valueAvailable = !string.IsNullOrEmpty(Value) && NumericParsingHelper.TryParseFloat(Value, out value);

            float fittingValue = 0;
            bool fittingValueAvailable = !string.IsNullOrEmpty(FittingValue) && NumericParsingHelper.TryParseFloat(FittingValue, out fittingValue);

            return new CustomerProfileSizeValue {
                Id = Id,
                Value = valueAvailable ? (float?)value: null,
                FittingValue = fittingValueAvailable ? (float?)fittingValue : null,
                MeasurementDefinitionId = MeasurementDefinitionId
            };
        }
    }
}
