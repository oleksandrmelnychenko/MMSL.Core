﻿using MMSL.Domain.Entities.Addresses;
using System.ComponentModel.DataAnnotations;

namespace MMSL.Domain.Entities.Dealer {
    public class DealerAccount : EntityBaseNamed {

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string AlternateEmail { get; set; }

        public string PhoneNumber { get; set; }

        public string TaxNumber { get; set; }

        public bool IsVatApplicable { get; set; }

        public int Currency { get; set; }

        public int PaymentType { get; set; }
        
        public bool IsCreditAllowed { get; set; }
        
        public bool? BillingAddressId { get; set; }

        public Address BillingAddress { get; set; }

        public bool UseBillingAsShipping { get; set; }

        public bool? ShippingAddressId { get; set; }

        public Address ShippingAddress { get; set; }
    }
}
