using MMSL.Domain.Entities.Identity;

namespace MMSL.Domain.Entities.Fabrics {
    public class Fabric : EntityBase {
        [Header("Fabric Code")]
        public string FabricCode { get; set; }

        [Header("Description")]
        public string Description { get; set; }

        [Header("Design Package Owner")]
        public string ImageUrl { get; set; }

        [Header("Status")]
        public FabricStatuses Status { get; set; }

        [Header("Meters")]
        public float? Metres { get; set; }
        public bool IsMetresVisible { get; set; } = true;

        [Header("Mill")]
        public string Mill { get; set; }
        public bool IsMillVisible { get; set; } = true;

        [Header("Color")]
        public string Color { get; set; }
        public bool IsColorVisible { get; set; } = true;

        [Header("Composition")]
        public string Composition { get; set; }
        public bool IsCompositionVisible { get; set; } = true;

        [Header("GSM")]
        public string GSM { get; set; }
        public bool IsGSMVisible { get; set; } = true;

        [Header("Count")]
        public string Count { get; set; }
        public bool IsCountVisible { get; set; } = true;

        [Header("Weave")]
        public string Weave { get; set; }
        public bool IsWeaveVisible { get; set; } = true;

        [Header("Pattern")]
        public string Pattern { get; set; }
        public bool IsPatternVisible { get; set; } = true;

        public long UserIdentityId { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
