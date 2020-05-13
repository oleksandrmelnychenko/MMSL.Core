using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Stores;
using System;
using System.ComponentModel.DataAnnotations;

namespace MMSL.Domain.Entities.StoreCustomers {
    public class StoreCustomer : EntityBase {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public bool UseBillingAsDeliveryAddress { get; set; }

        public long? BillingAddressId { get; set; }
        public Address BillingAddress { get; set; }

        public long? DeliveryAddressId { get; set; }
        public Address DeliveryAddress { get; set; }

        public long StoreId { get; set; }
        public Store Store { get; set; }
    }
}
