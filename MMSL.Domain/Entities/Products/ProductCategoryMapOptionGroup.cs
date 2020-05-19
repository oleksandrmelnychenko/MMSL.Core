using MMSL.Domain.Entities.Options;

namespace MMSL.Domain.Entities.Products {
    public class ProductCategoryMapOptionGroup : EntityBase {
        public long ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public long OptionGroupId { get; set; }
        public OptionGroup OptionGroup { get; set; }
    }
}
