using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Measurements;

namespace MMSL.Databases.TableMaps.Measurements {
    internal class FittingTypeMap : EntityBaseMap<FittingType> {
        public override void Map(EntityTypeBuilder<FittingType> entity) {
            base.Map(entity);

            entity.ToTable("FittingTypes");
        }
    }
}
