using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Stores {
    public class Store : EntityBaseNamed {

        public long AddressId { get; set; }
        public Address Address { get; set; }

        [Required]
        public string ContactEmail { get; set; }

        [Required]
        public string BillingEmail { get; set; }

        public ICollection<StoreMapDealerAccount> StoreMapDealerAccounts { get; set; }

        [NotMapped]
        public int StoreCustomersCount { get; set; }
    }
}
