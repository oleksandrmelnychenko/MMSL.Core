using MMSL.Domain.Entities.Products;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IProductPermissionSettingsRepository {
        ProductPermissionSettings AddProductPermissionSettings(ProductPermissionSettings productSettings);
        ProductPermissionSettings GetProductPermissionSettingsById(long productSettingsId, bool includeDeletedSettings = false);
        List<ProductPermissionSettings> GetProductPermissionSettingsByProduct(long productId);
        ProductPermissionSettings UpdateProductPermissionSettings(ProductPermissionSettings productSettings);
    }
}
