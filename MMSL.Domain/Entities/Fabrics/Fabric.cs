using MMSL.Domain.Entities.Identity;

namespace MMSL.Domain.Entities.Fabrics {
    public class Fabric : EntityBase {
        public string FabricCode { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public FabricStatuses Status { get; set; }

        public string Metres { get; set; }
        public bool IsMetresVisible { get; set; }

        public string Mill { get; set; }
        public bool IsMillVisible { get; set; }
        
        public string Color { get; set; }
        public bool IsColorVisible { get; set; }
        
        public string Composition { get; set; }
        public bool IsCompositionVisible { get; set; }
        
        public string GSM { get; set; }
        public bool IsGSMVisible { get; set; }
        
        public int Count { get; set; }
        public bool IsCountVisible { get; set; }
        
        public string Weave { get; set; }
        public bool IsWeaveVisible { get; set; }
        
        public string Pattern { get; set; }
        public bool IsPatternVisible { get; set; }

        public long UserIdentityId { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
