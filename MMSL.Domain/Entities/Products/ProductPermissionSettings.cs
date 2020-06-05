using MMSL.Domain.Entities.Dealer;
using System.Collections.Generic;

namespace MMSL.Domain.Entities.Products {
    public class ProductPermissionSettings : EntityBaseNamed {

        public long ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public ICollection<PermissionSettings> PermissionSettings { get; set; } = new HashSet<PermissionSettings>();
        public ICollection<DealerMapProductPermissionSettings> DealerMapProductPermissionSettings { get; set; } = new HashSet<DealerMapProductPermissionSettings>();
    }
}
