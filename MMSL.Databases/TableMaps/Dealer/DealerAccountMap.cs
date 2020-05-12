using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Dealer;

namespace MMSL.Databases.TableMaps.Dealer {
    public  class DealerAccountMap : EntityBaseMap<DealerAccount> {
        public override void Map(EntityTypeBuilder<DealerAccount> entity) {
            base.Map(entity);

            entity.ToTable("DealerAccount");
        }
    }
}
