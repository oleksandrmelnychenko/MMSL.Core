using MMSL.Domain.Entities.StoreCustomers;
using System;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Customer {
    public class NewCustomerProductProfile {

        public string Name { get; set; }

        public string Description { get; set; }

        public long ProductCategoryId { get; set; }

        public long StoreCustomerId { get; set; }

        public long? MeasurementId { get; set; }

        public long? MeasurementSizeId { get; set; }

        public long? FittingTypeId { get; set; }

        public List<NewCustomerProfileValueDataContract> Values { get; set; } = new List<NewCustomerProfileValueDataContract>();

        public CustomerProductProfile GetEntity() {
            return new CustomerProductProfile() {
                Name = Name,
                Description = Description,
                ProductCategoryId = ProductCategoryId,
                StoreCustomerId = StoreCustomerId,
                MeasurementId = MeasurementId,
                MeasurementSizeId = MeasurementSizeId,
                FittingTypeId = FittingTypeId
            };
        }
    }
}
