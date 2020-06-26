using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Customer {
    public class UpdateCustomerProductProfile : NamedEntityDataContractBase<CustomerProductProfile> {

        public long? MeasurementId { get; set; }

        public long? FittingTypeId { get; set; }

        public List<UpdateCustomerProfileValueDataContract> Values { get; set; } = new List<UpdateCustomerProfileValueDataContract>();

        public override CustomerProductProfile GetEntity() {
            return new CustomerProductProfile() {
                Name = Name,
                Description = Description
            };
        }
    }
}
