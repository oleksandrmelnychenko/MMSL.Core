using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    public class MeasurementMapSizeMap : EntityBaseMap<MeasurementMapSize> {
        public override void Map(EntityTypeBuilder<MeasurementMapSize> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementMapSizes");

            entity.HasOne(x => x.Measurement)
                .WithMany(x => x.MeasurementMapSizes)
                .HasForeignKey(x => x.MeasurementId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MeasurementSize)
                .WithMany(x => x.MeasurementMapSizes)
                .HasForeignKey(x => x.MeasurementSizeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
