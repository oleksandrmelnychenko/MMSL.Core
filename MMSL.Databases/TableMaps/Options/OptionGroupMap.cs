using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Options;

namespace MMSL.Databases.TableMaps.Options {
    public class OptionGroupMap : EntityBaseMap<OptionGroup> {
        public override void Map(EntityTypeBuilder<OptionGroup> entity) {
            base.Map(entity);
            entity.ToTable("OptionGroups");
        }
    }
}
