using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.Entities.Products {
    public class ProductCategory : EntityBaseNamed {

        public string ImageUrl { get; set; }

        public ProductCategory() {
            DeliveryTimelineProductMaps = new HashSet<DeliveryTimelineProductMap>();
            OptionGroupMaps = new HashSet<ProductCategoryMapOptionGroup>();
            Measurements = new HashSet<Measurement>();
        }

        public ICollection<DeliveryTimelineProductMap> DeliveryTimelineProductMaps { get; set; }

        public ICollection<ProductCategoryMapOptionGroup> OptionGroupMaps { get; set; }

        public ICollection<Measurement> Measurements { get; set; }
    }
}
