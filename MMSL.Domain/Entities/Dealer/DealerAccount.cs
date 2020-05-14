﻿using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Stores;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MMSL.Domain.Entities.Dealer {
    public class DealerAccount : EntityBase {


        [Required]
        public string Name { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string AlternateEmail { get; set; }

        public string PhoneNumber { get; set; }

        public string TaxNumber { get; set; }

        public bool IsVatApplicable { get; set; }

        public int Currency { get; set; }

        public int PaymentType { get; set; }

        public bool IsCreditAllowed { get; set; }

        public long? BillingAddressId { get; set; }

        public Address BillingAddress { get; set; }

        public bool UseBillingAsShipping { get; set; }

        public long? ShippingAddressId { get; set; }

        public Address ShippingAddress { get; set; }

        public ICollection<StoreMapDealerAccount> StoreMapDealerAccounts { get; set; }
    }
}
