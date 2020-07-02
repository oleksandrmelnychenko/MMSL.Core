using MMSL.Domain.Entities.Products;
using System.Collections.Generic;
using System.Linq;

namespace MMSL.Domain.DataContracts.Products {
    public class NewProductPermissionSettingsDataContract : EntityDataContractBase<ProductPermissionSettings> {

        public string Name { get; set; }

        public string Description { get; set; }

        public long ProductCategoryId { get; set; }

        public bool IsDefault { get; set; }

        public List<NewPermissionSettingDataContract> PermissionSettings { get; set; } = new List<NewPermissionSettingDataContract>();

        public override ProductPermissionSettings GetEntity() {
            return new ProductPermissionSettings {
                Name = Name,
                Description = Description,
                ProductCategoryId = ProductCategoryId,
                IsDefault = IsDefault
            };
        }
    }
}
