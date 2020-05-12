using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts {
    public class NewStoreDataContract {
        public string Name { get; set; }

        public long DealerAccountId { get; set; }
        public DealerAccount DealerAccount { get; set; }

        public long AddressId { get; set; }
        public Address Address { get; set; }
      
        public string ContactEmail { get; set; }
     
        public string BillingEmail { get; set; }
    }
}
