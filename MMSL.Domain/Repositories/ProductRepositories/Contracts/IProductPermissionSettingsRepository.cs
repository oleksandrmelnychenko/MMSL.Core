using MMSL.Domain.Entities.Products;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IProductPermissionSettingsRepository {
        ProductPermissionSettings AddProductPermissionSettings(ProductPermissionSettings productSettings);
        ProductPermissionSettings GetProductPermissionSettingsById(long productSettingsId);
        ProductPermissionSettings GetProductPermissionSettings(long productId);
        ProductPermissionSettings UpdateProductPermissionSettings(long productSettingsId);
    }
}
