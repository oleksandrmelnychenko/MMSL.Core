using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Stores;

namespace MMSL.Databases.TableMaps.Stores {
    public class StoreMap : EntityBaseMap<Store> {
        public override void Map(EntityTypeBuilder<Store> entity) {
            base.Map(entity);
            entity.ToTable("Stores");
        }
    }
}

