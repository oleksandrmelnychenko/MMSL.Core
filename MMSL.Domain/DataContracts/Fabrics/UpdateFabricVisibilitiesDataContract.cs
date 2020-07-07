using MMSL.Domain.Entities.Fabrics;

namespace MMSL.Domain.DataContracts.Fabrics {
    public class UpdateFabricVisibilitiesDataContract {
        public long Id { get; set; }

        public bool IsMetresVisible { get; set; }
        public bool IsMillVisible { get; set; }
        public bool IsColorVisible { get; set; }
        public bool IsCompositionVisible { get; set; }
        public bool IsGSMVisible { get; set; }
        public bool IsCountVisible { get; set; }
        public bool IsWeaveVisible { get; set; }
        public bool IsPatternVisible { get; set; }

        public Fabric MapFabric(Fabric exist) {
            exist.IsColorVisible = IsMetresVisible;
            exist.IsColorVisible = IsMillVisible;
            exist.IsColorVisible = IsColorVisible;
            exist.IsColorVisible = IsCompositionVisible;
            exist.IsColorVisible = IsGSMVisible;
            exist.IsColorVisible = IsCountVisible;
            exist.IsColorVisible = IsWeaveVisible;
            exist.IsColorVisible = IsPatternVisible;

            return exist;
        }
    }
}
