using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    public class MeasurementMap : EntityBaseMap<Measurement> {
        public override void Map(EntityTypeBuilder<Measurement> entity) {
            base.Map(entity);

            entity.ToTable("Measurements");
        }
    }
}
