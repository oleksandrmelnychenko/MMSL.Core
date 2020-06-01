using MMSL.Domain.Entities.Products;

namespace MMSL.Domain.Entities.DeliveryTimelines {
    public class DeliveryTimelineProductMap : EntityBase {
        public long DeliveryTimelineId { get; set; }
        public DeliveryTimeline DeliveryTimeline { get; set; }

        public long ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
