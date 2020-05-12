using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
using System.ComponentModel.DataAnnotations;

namespace MMSL.Domain.Entities.Stores {
    public class Store : EntityBaseNamed {
        public long DealerAccountId { get; set; }
        public DealerAccount DealerAccount { get; set; }

        public long AddressId { get; set; }
        public Address Address { get; set; }

        [Required]
        public string ContactEmail { get; set; }

        [Required]
        public string BillingEmail { get; set; }
    }
}
