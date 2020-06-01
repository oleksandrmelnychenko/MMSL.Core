using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.DeliveryTimelines;

namespace MMSL.Databases.TableMaps.DeliveryTimelines {
    internal class DeliveryTimelineProductMapMap : EntityBaseMap<DeliveryTimelineProductMap> {

        public override void Map(EntityTypeBuilder<DeliveryTimelineProductMap> entity) {
            base.Map(entity);
            entity.ToTable("DeliveryTimelineProductMaps");

            entity.HasOne(x => x.DeliveryTimeline)
                .WithMany(x => x.DeliveryTimelineProductMaps)
                .HasForeignKey(x => x.DeliveryTimelineId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.ProductCategory)
                .WithMany(x => x.DeliveryTimelineProductMaps)
                .HasForeignKey(x => x.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
