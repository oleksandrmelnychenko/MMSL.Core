﻿using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Entities.StoreCustomers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public bool IsDisabled { get; set; }

        public ICollection<CustomerProductProfile> CustomerProductProfiles { get; set; } = new HashSet<CustomerProductProfile>();
    }
}
