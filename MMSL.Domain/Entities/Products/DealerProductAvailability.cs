using MMSL.Domain.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Entities.Products {
    public class DealerProductAvailability : EntityBase {
        
        public long DealerAccountId { get; set; }

        public DealerAccount DealerAccount { get; set; }

        public long ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public bool IsDisabled { get; set; }

    }
}
