using MMSL.Domain.Entities.Options;

namespace MMSL.Domain.Entities.Products {
    public class PermissionSettings : EntityBase {

        public bool IsAllow { get; set; }

        public long ProductPermissionSettingsId { get; set; }
        public ProductPermissionSettings ProductPermissionSettings { get; set; }

        public long OptionGroupId { get; set; }
        public OptionGroup OptionGroup { get; set; }

        public long? OptionUnitId { get; set; }
        public OptionUnit OptionUnit { get; set; }
    }
}
