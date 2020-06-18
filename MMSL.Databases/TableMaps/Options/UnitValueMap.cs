using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Options;

namespace MMSL.Databases.TableMaps.Options {
    public class UnitValueMap : EntityBaseMap<UnitValue> {
        public override void Map(EntityTypeBuilder<UnitValue> entity) {
            base.Map(entity);
            entity.ToTable("UnitValues");
        }
    }
}
