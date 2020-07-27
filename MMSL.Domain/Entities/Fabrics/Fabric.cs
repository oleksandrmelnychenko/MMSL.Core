using MMSL.Domain.Entities.Identity;

namespace MMSL.Domain.Entities.Fabrics {
    public class Fabric : EntityBase {
        public string FabricCode { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public FabricStatuses Status { get; set; }

        public float? Metres { get; set; }
        public bool IsMetresVisible { get; set; } = true;

        public string Mill { get; set; }
        public bool IsMillVisible { get; set; } = true;

        public string Color { get; set; }
        public bool IsColorVisible { get; set; } = true;

        public string Composition { get; set; }
        public bool IsCompositionVisible { get; set; } = true;

        public string GSM { get; set; }
        public bool IsGSMVisible { get; set; } = true;

        public string Count { get; set; }
        public bool IsCountVisible { get; set; } = true;

        public string Weave { get; set; }
        public bool IsWeaveVisible { get; set; } = true;

        public string Pattern { get; set; }
        public bool IsPatternVisible { get; set; } = true;

        public long UserIdentityId { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
