using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Products;

namespace MMSL.Databases.TableMaps.Products {
    public class ProductPermissionSettingsMap : EntityBaseMap<ProductPermissionSettings> {
        public override void Map(EntityTypeBuilder<ProductPermissionSettings> entity) {
            base.Map(entity);

            entity.ToTable("ProductPermissionSettings");
        }
    }
}
