using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts.Products {
    public class UpdatePermissionSettingDataContract {
        public long Id { get; set; }
        public bool IsAllow { get; set; }
        public long OptionGroupId { get; set; }
        public long? OptionUnitId { get; set; }
    }
}
