using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    class MeasurementDefinitionMap : EntityBaseMap<MeasurementDefinition> {
        public override void Map(EntityTypeBuilder<MeasurementDefinition> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementDefinitions");
        }
    }
}
