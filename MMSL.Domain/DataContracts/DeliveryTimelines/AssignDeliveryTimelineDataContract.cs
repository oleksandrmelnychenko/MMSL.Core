using MMSL.Domain.Entities.DeliveryTimelines;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.DeliveryTimelines {
    public class AssignDeliveryTimelineDataContract {
        public long ProductCategoryId { get; set; }

        public List<DeliveryTimeline> DeliveryTimelines { get; set; }
    }
}
