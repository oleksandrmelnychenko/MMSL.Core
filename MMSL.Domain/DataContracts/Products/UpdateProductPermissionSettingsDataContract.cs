using MMSL.Domain.Entities.Products;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Products {
    public class UpdateProductPermissionSettingsDataContract : EntityDataContractBase<ProductPermissionSettings> {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }

        public List<UpdatePermissionSettingDataContract> PermissionSettings { get; set; } = new List<UpdatePermissionSettingDataContract>();

        public override ProductPermissionSettings GetEntity() {
            return new ProductPermissionSettings {
                Id = Id,
                Name = Name,
                Description = Description,
                IsDefault = IsDefault
            };
        }
    }
}
