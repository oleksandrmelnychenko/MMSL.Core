using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Addresses;

namespace MMSL.Databases.TableMaps.Addresses {
    public class AddressMap : EntityBaseMap<Address> {
        public override void Map(EntityTypeBuilder<Address> entity) {
            base.Map(entity);

            entity.ToTable("Address");
        }
    }
}
