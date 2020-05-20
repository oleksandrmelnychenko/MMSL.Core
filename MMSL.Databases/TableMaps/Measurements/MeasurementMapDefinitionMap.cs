using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    class MeasurementMapDefinitionMap : EntityBaseMap<MeasurementMapDefinition> {
        public override void Map(EntityTypeBuilder<MeasurementMapDefinition> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementMapDefinitions");

            entity
                .HasOne(x => x.Measurement)
                .WithMany(x => x.MeasurementMapDefinitions)
                .HasForeignKey(x => x.MeasurementId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(x => x.MeasurementDefinition)
                .WithMany(x => x.MeasurementMapDefinitions)
                .HasForeignKey(x => x.MeasurementDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
