using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Customer {
    public class UpdateCustomerProductProfile : NamedEntityDataContractBase<CustomerProductProfile> {

        public long? MeasurementId { get; set; }

        public long? FittingTypeId { get; set; }

        public long? MeasurementSizeId { get; set; }

        public ProfileTypes ProfileType { get; set; }

        public List<UpdateCustomerProfileValueDataContract> Values { get; set; } = new List<UpdateCustomerProfileValueDataContract>();

        public List<UpdateProfileProductStyleConfigurationDataContract> ProductStyles { get; set; } = new List<UpdateProfileProductStyleConfigurationDataContract>();

        public override CustomerProductProfile GetEntity() {
            return new CustomerProductProfile() {
                Id = Id,
                Name = Name,
                Description = Description,
                MeasurementId = MeasurementId.HasValue && MeasurementId == default(long) ? null : MeasurementId,
                MeasurementSizeId = MeasurementSizeId.HasValue && MeasurementSizeId == default(long) ? null : MeasurementSizeId,
                FittingTypeId = FittingTypeId.HasValue && FittingTypeId == default(long) ? null: FittingTypeId,
                ProfileType = ProfileType
            };
        }
    }
}
