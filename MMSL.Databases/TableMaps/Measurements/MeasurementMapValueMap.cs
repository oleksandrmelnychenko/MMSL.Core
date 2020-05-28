using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    internal class MeasurementMapValueMap : EntityBaseMap<MeasurementMapValue> {
        public override void Map(EntityTypeBuilder<MeasurementMapValue> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementMapValues");

            entity.HasOne(x => x.Measurement)
                .WithMany(x => x.MeasurementMapValues)
                .HasForeignKey(x => x.MeasurementId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MeasurementSize)
                .WithMany(x => x.MeasurementMapValues)
                .HasForeignKey(x => x.MeasurementSizeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MeasurementDefinition)
                .WithMany(x => x.MeasurementMapValues)
                .HasForeignKey(x => x.MeasurementDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.FittingType)
                .WithMany(x => x.MeasurementMapValues)
                .HasForeignKey(x => x.FittingTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
