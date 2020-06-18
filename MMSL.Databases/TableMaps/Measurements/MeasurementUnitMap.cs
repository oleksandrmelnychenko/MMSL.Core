using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    class MeasurementUnitMap : EntityBaseMap<MeasurementUnit> {
        public override void Map(EntityTypeBuilder<MeasurementUnit> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementUnits");
        }
    }
}
