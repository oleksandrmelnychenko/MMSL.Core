using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts.Products {
    public class ProductPermissionToDealersBindingDataContract {
        public long ProductPermissionSettingId { get; set; }
        public List<DealerIdDataContract> Dealers { get; set; }
    }

    public class DealerIdDataContract {
        public long DealerAccountId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
