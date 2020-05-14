using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.CurrencyTypes;

namespace MMSL.Databases.TableMaps.CurrencyTypes {
    public class CurrencyTypeMap : EntityBaseMap<CurrencyType> {
        public override void Map(EntityTypeBuilder<CurrencyType> entity) {
            base.Map(entity);

            entity.ToTable("CurrencyTypes");
        }
    }
}
