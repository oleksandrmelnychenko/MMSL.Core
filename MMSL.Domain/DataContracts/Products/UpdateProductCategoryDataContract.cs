using MMSL.Domain.Entities.Products;

namespace MMSL.Domain.DataContracts.Measurements {
    public class UpdateProductCategoryDataContract : NamedEntityDataContractBase<ProductCategory> {

        public string ImageUrl { get; set; }

        public override ProductCategory GetEntity() {
            return new ProductCategory {
                Id = Id,
                Name = Name,
                Description = Description,
                ImageUrl = ImageUrl
            };
        }
    }
}
