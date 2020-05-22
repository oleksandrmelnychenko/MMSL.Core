using MMSL.Domain.Entities.Measurements;
using System.Collections;
using System.Collections.Generic;

namespace MMSL.Domain.Entities.Products {
    public class ProductCategory : EntityBaseNamed {

        public string ImageUrl { get; set; }

        public ProductCategory() {
            OptionGroupMaps = new HashSet<ProductCategoryMapOptionGroup>();
        }

        public ICollection<ProductCategoryMapOptionGroup> OptionGroupMaps { get; set; }

        public ICollection<Measurement> Measurements { get; set; }
    }
}
