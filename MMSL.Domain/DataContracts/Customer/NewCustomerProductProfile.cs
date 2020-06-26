using MMSL.Domain.Entities.StoreCustomers;
using System;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Customer {
    public class NewCustomerProductProfile {

        public string Name { get; set; }

        public string Description { get; set; }

        public long ProductCategoryId { get; set; }

        public long StoreCustomerId { get; set; }

        public List<NewCustomerProfileValueDataContract> Values { get; set; }

        public CustomerProductProfile GetEntity() {
            return new CustomerProductProfile() {
                Name = Name,
                Description = Description,
                ProductCategoryId = ProductCategoryId,
                StoreCustomerId = StoreCustomerId
            };
        }
    }
}
