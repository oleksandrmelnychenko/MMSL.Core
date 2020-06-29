using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.StoreCustomers;

namespace MMSL.Databases.TableMaps.StoreCustomers {
    public class CustomerProfileStyleConfigurationMap : EntityBaseMap<CustomerProfileStyleConfiguration> {
        public override void Map(EntityTypeBuilder<CustomerProfileStyleConfiguration> entity) {
            base.Map(entity);

            entity.ToTable("CustomerProfileStyleConfigurations");
        }
    }
}
