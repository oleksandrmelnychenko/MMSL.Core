using MMSL.Domain.Entities.Products;
using System.Collections;
using System.Collections.Generic;

namespace MMSL.Domain.Entities.Options {
    public class OptionGroup : EntityBase {

        public OptionGroup() {
            OptionUnits = new HashSet<OptionUnit>();
            ProductCategoryMaps = new HashSet<ProductCategoryMapOptionGroup>();
        }

        public string Name { get; set; }

        public bool IsMandatory { get; set; }

        public ICollection<OptionUnit> OptionUnits { get; set; }

        public ICollection<ProductCategoryMapOptionGroup> ProductCategoryMaps { get; set; }
    }
}
