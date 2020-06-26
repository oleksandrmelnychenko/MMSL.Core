using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Databases.TableMaps.StoreCustomers {
    public class CustomerProductProfileMap : EntityBaseMap<CustomerProductProfile> {
        public override void Map(EntityTypeBuilder<CustomerProductProfile> entity) {
            base.Map(entity);

            entity.ToTable("CustomerProductProfiles");
        }
    }
}
