using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Options;

namespace MMSL.Databases.TableMaps.Options {
    public class OptionPriceMap : EntityBaseMap<OptionPrice> {
        public override void Map(EntityTypeBuilder<OptionPrice> entity) {
            base.Map(entity);

            entity.ToTable("OptionPrices");
        }
    }
}
