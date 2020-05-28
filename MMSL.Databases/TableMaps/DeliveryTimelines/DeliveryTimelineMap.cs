using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.DeliveryTimelines;

namespace MMSL.Databases.TableMaps.DeliveryTimelines {
    internal class DeliveryTimelineMap : EntityBaseMap<DeliveryTimeline> {
        public override void Map(EntityTypeBuilder<DeliveryTimeline> entity) {
            base.Map(entity);
            entity.ToTable("DeliveryTimelines");
        }
    }
}
