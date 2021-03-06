﻿using MMSL.Domain.Entities.Products;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Measurements {
    public class NewProductCategoryDataContract : NamedEntityDataContractBase<ProductCategory> {
        public List<long> OptionGroupIds { get; set; }

        public override ProductCategory GetEntity() {
            return new ProductCategory {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }
    }
}
