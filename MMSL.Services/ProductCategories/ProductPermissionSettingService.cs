using MMSL.Domain.DataContracts.Products;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using MMSL.Services.ProductCategories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories {
    public class ProductPermissionSettingService : IProductPermissionSettingService {

        private readonly IProductCategoryRepositoriesFactory _productCategoryRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public ProductPermissionSettingService(
            IDbConnectionFactory connectionFactory,
            IProductCategoryRepositoriesFactory productCategoryRepositoriesFactory
            ) {
            _connectionFactory = connectionFactory;
            _productCategoryRepositoriesFactory = productCategoryRepositoriesFactory;
        }

        public Task<ProductPermissionSettings> AddProductPermissionSetting(NewProductPermissionSettingsDataContract productPermissionSettings) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductPermissionSettingsRepository productSettingsRepository = _productCategoryRepositoriesFactory.NewProductPermissionSettingsRepository(connection);
                    IPermissionSettingsRepository settingsRepository = _productCategoryRepositoriesFactory.NewPermissionSettingsRepository(connection);

                    ProductPermissionSettings settingEntity = productSettingsRepository.AddProductPermissionSettings(productPermissionSettings.GetEntity());

                    foreach (NewPermissionSettingDataContract setting in productPermissionSettings.PermissionSettings) {

                        var detailEntity = settingsRepository.AddPermissionSettings(
                            new PermissionSettings {
                                IsAllow = setting.IsAllow,
                                OptionGroupId = setting.OptionGroupId,
                                OptionUnitId = setting.OptionUnitId,
                                ProductPermissionSettingsId = settingEntity.Id
                            });

                        settingEntity.PermissionSettings.Add(detailEntity);
                    }

                    return settingEntity;
                }
            });

        public Task<ProductPermissionSettings> DeletePermissionSettingsById(long productPermissionSettingId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductPermissionSettingsRepository repository = _productCategoryRepositoriesFactory.NewProductPermissionSettingsRepository(connection);

                    ProductPermissionSettings permissionSetting = repository.GetProductPermissionSettingsById(productPermissionSettingId);

                    if (permissionSetting == null) {
                        //TODO: throw not found
                    }

                    permissionSetting.IsDeleted = true;

                    return repository.UpdateProductPermissionSettings(permissionSetting);
                }
            });

        public Task<ProductPermissionSettings> GetPermissionSettingsById(long productPermissionSettingId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _productCategoryRepositoriesFactory
                        .NewProductPermissionSettingsRepository(connection)
                        .GetProductPermissionSettingsById(productPermissionSettingId);
                }
            });

        public Task<List<ProductPermissionSettings>> GetSettingsByProduct(long productCategoryId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _productCategoryRepositoriesFactory
                        .NewProductPermissionSettingsRepository(connection)
                        .GetProductPermissionSettingsByProduct(productCategoryId);
                }
            });

        public Task<ProductPermissionSettings> UpdateProductPermissionSetting(UpdateProductPermissionSettingsDataContract productPermissionSettings) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductPermissionSettingsRepository productSettingsRepository = _productCategoryRepositoriesFactory.NewProductPermissionSettingsRepository(connection);
                    IPermissionSettingsRepository settingsRepository = _productCategoryRepositoriesFactory.NewPermissionSettingsRepository(connection);

                    ProductPermissionSettings productSettings = productSettingsRepository.UpdateProductPermissionSettings(productPermissionSettings.GetEntity());

                    foreach (UpdatePermissionSettingDataContract settings in productPermissionSettings.PermissionSettings) {
                        var updatedEntity = settingsRepository
                            .UpdatePermissionSettings(
                                new PermissionSettings {
                                    Id = settings.Id,
                                    IsAllow = settings.IsAllow
                                });

                        productSettings.PermissionSettings.Add(updatedEntity);
                    }

                    return productSettings;
                }
            });
    }
}
