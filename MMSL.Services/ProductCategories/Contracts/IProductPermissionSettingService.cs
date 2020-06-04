using MMSL.Domain.DataContracts.Products;
using MMSL.Domain.Entities.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories.Contracts {
    public interface IProductPermissionSettingService {
        Task<List<ProductPermissionSettings>> GetSettingsByProduct(long productCategoryId);
        Task<ProductPermissionSettings> AddProductPermissionSetting(NewProductPermissionSettingsDataContract productPermissionSettings);
        Task<ProductPermissionSettings> GetPermissionSettingsById(long productPermissionSettingId);
        Task<ProductPermissionSettings> DeletePermissionSettingsById(long productPermissionSettingId);
        Task<ProductPermissionSettings> UpdateProductPermissionSetting(NewProductPermissionSettingsDataContract productPermissionSettings);
    }
}
