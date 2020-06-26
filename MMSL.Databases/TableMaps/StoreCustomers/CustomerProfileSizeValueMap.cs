using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Databases.TableMaps.StoreCustomers {
    public class CustomerProfileSizeValueMap : EntityBaseMap<CustomerProfileSizeValue> {
        public override void Map(EntityTypeBuilder<CustomerProfileSizeValue> entity) {
            base.Map(entity);

            entity.ToTable("CustomerProfileSizeValues");
        }
    }
}
