using MMSL.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Entities.Dealer {
    public class DealerMapProductPermissionSettings : EntityBase {
        public long DealerAccountId { get; set; }
        public DealerAccount DealerAccount { get; set; }
        
        public long ProductPermissionSettingsId { get; set; }
        public ProductPermissionSettings ProductPermissionSettings { get; set; }
    }

}
