using MMSL.Domain.Entities.Fabrics;

namespace MMSL.Domain.DataContracts.Fabrics {
    public class FabricVisibilitiesDataContract {
        public bool IsMetresVisible { get; set; }
        public bool IsMillVisible { get; set; }
        public bool IsColorVisible { get; set; }
        public bool IsCompositionVisible { get; set; }
        public bool IsGSMVisible { get; set; }
        public bool IsCountVisible { get; set; }
        public bool IsWeaveVisible { get; set; }
        public bool IsPatternVisible { get; set; }
    }
}
