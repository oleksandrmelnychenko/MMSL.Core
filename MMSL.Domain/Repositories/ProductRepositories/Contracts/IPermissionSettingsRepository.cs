using MMSL.Domain.Entities.Products;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IPermissionSettingsRepository {
        PermissionSettings AddPermissionSettings(PermissionSettings settings);
        //PermissionSettings GetPermissionSettings(long productSettingsId);
        PermissionSettings GetPermissionSettingsById(long settingsId);
        PermissionSettings UpdatePermissionSettings(PermissionSettings settings);
        PermissionSettings GetPermissionSettingsByOptionUnit(long id, long optionUnitId);
    }
}
