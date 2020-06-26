using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Domain.DataContracts.Customer {
    public class UpdateCustomerProfileValueDataContract : EntityDataContractBase<CustomerProfileSizeValue> {
        /// <summary>
        ///     Body measurement
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        ///     Add to body measurement
        /// </summary>
        public float? FittingValue { get; set; }

        public override CustomerProfileSizeValue GetEntity() {
            return new CustomerProfileSizeValue {
                Id = Id,
                Value = Value,
                FittingValue = FittingValue
            };
        }
    }
}
