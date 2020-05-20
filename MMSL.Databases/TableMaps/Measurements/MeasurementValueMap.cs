using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    class MeasurementValueMap : EntityBaseMap<MeasurementValue> {
        public override void Map(EntityTypeBuilder<MeasurementValue> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementValues");
        }
    }
}
