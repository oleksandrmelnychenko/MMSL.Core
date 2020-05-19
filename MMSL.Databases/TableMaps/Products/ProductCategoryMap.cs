using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Products;

namespace MMSL.Databases.TableMaps.Products {
    public class ProductCategoryMap : EntityBaseMap<ProductCategory> {
        public override void Map(EntityTypeBuilder<ProductCategory> entity) {
            base.Map(entity);

            entity.ToTable("ProductCategories");
        }
    }
}
