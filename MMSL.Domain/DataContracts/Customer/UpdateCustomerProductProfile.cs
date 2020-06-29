using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Customer {
    public class UpdateCustomerProductProfile : NamedEntityDataContractBase<CustomerProductProfile> {

        public long? MeasurementId { get; set; }

        public long? FittingTypeId { get; set; }

        public long? MeasurementSizeId { get; set; }

        public ProfileTypes ProfileType { get; set; }

        public List<UpdateCustomerProfileValueDataContract> Values { get; set; } = new List<UpdateCustomerProfileValueDataContract>();

        public override CustomerProductProfile GetEntity() {
            return new CustomerProductProfile() {
                Id = Id,
                Name = Name,
                Description = Description,
                MeasurementId = MeasurementId,
                MeasurementSizeId = MeasurementSizeId,
                FittingTypeId = FittingTypeId,
                ProfileType = ProfileType
            };
        }
    }
}
