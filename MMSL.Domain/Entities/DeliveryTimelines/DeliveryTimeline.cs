using System.Collections.Generic;

namespace MMSL.Domain.Entities.DeliveryTimelines {
    public class DeliveryTimeline : EntityBase {

        public DeliveryTimeline() {
            DeliveryTimelineProductMaps = new HashSet<DeliveryTimelineProductMap>();
        }

        public string Name { get; set; }

        public string Ivory { get; set; }

        public string Silver { get; set; }

        public string Black { get; set; }

        public string Gold { get; set; }

        public bool IsDefault { get; set; }

        public ICollection<DeliveryTimelineProductMap> DeliveryTimelineProductMaps { get; set; }
    }
}
