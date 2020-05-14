using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Options;

namespace MMSL.Databases.TableMaps.Options {
    public class OptionUnitMap : EntityBaseMap<OptionUnit> {
        public override void Map(EntityTypeBuilder<OptionUnit> entity) {
            base.Map(entity);

            entity.ToTable("OptionUnits");
        }
    }
}
