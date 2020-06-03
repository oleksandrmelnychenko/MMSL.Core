using System.Collections.Generic;

namespace MMSL.Domain.Entities.Products {
    public class ProductPermissionSettings : EntityBaseNamed {

        public long ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public ICollection<PermissionSettings> PermissionSettings { get; set; }
    }
}
