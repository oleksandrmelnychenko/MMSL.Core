using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    internal class MeasurementSizeMap : EntityBaseMap<MeasurementSize> {
        public override void Map(EntityTypeBuilder<MeasurementSize> entity) {
            base.Map(entity);

            entity.ToTable("MeasurementSizes");
        }
    }
}
