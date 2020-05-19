using System.Collections;
using System.Collections.Generic;

namespace MMSL.Domain.Entities.Products {
    public class ProductCategory : EntityBaseNamed {
        public ICollection<ProductCategoryMapOptionGroup> OptionGroupMaps { get; set; }
    }
}
