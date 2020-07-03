using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Databases.TableMaps.StoreCustomers {
    public class StoreCustomersMap : EntityBaseMap<StoreCustomer> {
        public override void Map(EntityTypeBuilder<StoreCustomer> entity) {
            base.Map(entity);

            entity.ToTable("StoreCustomers");

            entity
                .Property(x => x.UniqueId)
                .HasComputedColumnSql("[Id] + 10004000");
        }
    }
}
