using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Stores;

namespace MMSL.Databases.TableMaps.Stores {
    public class StoreDealerAccountMap : EntityBaseMap<StoreMapDealerAccount> {
        public override void Map(EntityTypeBuilder<StoreMapDealerAccount> entity) {
            base.Map(entity);

            entity.ToTable("StoreMapDealerAccounts");

            entity
                .HasOne(x => x.DealerAccount)
                .WithMany(x => x.StoreMapDealerAccounts)
                .HasForeignKey(x => x.DealerAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(x => x.Store)
                .WithMany(x => x.StoreMapDealerAccounts)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
