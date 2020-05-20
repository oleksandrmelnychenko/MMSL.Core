using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    class MeasurementSizeMap : EntityBaseMap<MeasurementSize> {
        public override void Map(EntityTypeBuilder<MeasurementSize> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementSizes");

            entity.HasOne(x => x.Measurement)
                .WithMany(x => x.MeasurementSizes)
                .HasForeignKey(s => s.MeasurementId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
