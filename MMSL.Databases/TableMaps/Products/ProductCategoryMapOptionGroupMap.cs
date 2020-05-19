using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Products;

namespace MMSL.Databases.TableMaps.Products {
    public class ProductCategoryMapOptionGroupMap : EntityBaseMap<ProductCategoryMapOptionGroup> {
        public override void Map(EntityTypeBuilder<ProductCategoryMapOptionGroup> entity) {
            base.Map(entity);

            entity.HasOne(x => x.ProductCategory)
                .WithMany(x => x.OptionGroupMaps)
                .HasForeignKey(x => x.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.OptionGroup)
                .WithMany(x => x.ProductCategoryMaps)
                .HasForeignKey(x => x.OptionGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
