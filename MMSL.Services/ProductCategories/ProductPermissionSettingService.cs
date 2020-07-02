using MMSL.Domain.DataContracts.Products;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.Dealer.Contracts;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using MMSL.Services.ProductCategories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories {
    public class ProductPermissionSettingService : IProductPermissionSettingService {

        private readonly IProductCategoryRepositoriesFactory _productCategoryRepositoriesFactory;
        private readonly IDealerRepositoriesFactory _dealerRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public ProductPermissionSettingService(
            IDbConnectionFactory connectionFactory,
            IProductCategoryRepositoriesFactory productCategoryRepositoriesFactory,
            IDealerRepositoriesFactory dealerRepositoriesFactory
            ) {
            _connectionFactory = connectionFactory;
            _productCategoryRepositoriesFactory = productCategoryRepositoriesFactory;
            _dealerRepositoriesFactory = dealerRepositoriesFactory;
        }

        public Task<ProductPermissionSettings> AddProductPermissionSetting(NewProductPermissionSettingsDataContract productPermissionSettings) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductPermissionSettingsRepository productSettingsRepository = _productCategoryRepositoriesFactory.NewProductPermissionSettingsRepository(connection);
                    IPermissionSettingsRepository settingsRepository = _productCategoryRepositoriesFactory.NewPermissionSettingsRepository(connection);

                    ProductPermissionSettings defaultPermission = productSettingsRepository.GetDefault(productPermissionSettings.ProductCategoryId);

                    if (productPermissionSettings.IsDefault && defaultPermission != null) {
                        defaultPermission.IsDefault = false;
                        productSettingsRepository.UpdateProductPermissionSettings(defaultPermission);
                    }

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

        public Task<List<ProductPermissionSettings>> GetSettingsByProduct(long productCategoryId, string dealerSearchTerm) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _productCategoryRepositoriesFactory
                        .NewProductPermissionSettingsRepository(connection)
                        .GetProductPermissionSettingsByProduct(productCategoryId, dealerSearchTerm);
                }
            });

        public Task SetDealerToProductPermissionSetting(ProductPermissionToDealersBindingDataContract productPermissionToDealersBinding) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IDealerAccountMapProductPermissionSettingsRepository repository = _dealerRepositoriesFactory.NewDealerAccountProductPermissionRepository(connection);

                    List<DealerMapProductPermissionSettings> bindings = repository.GetByProductPermissionSetting(productPermissionToDealersBinding.ProductPermissionSettingId);

                    foreach (DealerIdDataContract dealerIdDataContract in productPermissionToDealersBinding.Dealers) {
                        DealerMapProductPermissionSettings current = bindings.FirstOrDefault(x => x.DealerAccountId == dealerIdDataContract.DealerAccountId);

                        if (current != null) {
                            current.IsDeleted = dealerIdDataContract.IsDeleted;
                            repository.Update(current);
                        } else {
                            repository.Add(new DealerMapProductPermissionSettings {
                                DealerAccountId = dealerIdDataContract.DealerAccountId,
                                ProductPermissionSettingsId = productPermissionToDealersBinding.ProductPermissionSettingId
                            });
                        }
                    }
                }
            });

        public Task<ProductPermissionSettings> UpdateProductPermissionSetting(UpdateProductPermissionSettingsDataContract productPermissionSettings) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductPermissionSettingsRepository productSettingsRepository = _productCategoryRepositoriesFactory.NewProductPermissionSettingsRepository(connection);
                    IPermissionSettingsRepository settingsRepository = _productCategoryRepositoriesFactory.NewPermissionSettingsRepository(connection);

                    ProductPermissionSettings existPermission = productSettingsRepository.GetProductPermissionSettingsById(productPermissionSettings.Id);
                    ProductPermissionSettings defaultPermission = productSettingsRepository.GetDefault(existPermission.ProductCategoryId);

                    if (existPermission == null)
                        throw new Exception("Updating permission not found");

                    if (existPermission.IsDefault && defaultPermission != null && existPermission.Id != defaultPermission.Id) {
                        defaultPermission.IsDefault = false;
                        productSettingsRepository.UpdateProductPermissionSettings(defaultPermission);
                    }

                    ProductPermissionSettings productSettings = productSettingsRepository.UpdateProductPermissionSettings(productPermissionSettings.GetEntity());

                    foreach (UpdatePermissionSettingDataContract settingsDataContract in productPermissionSettings.PermissionSettings) {
                        PermissionSettings settings = productSettings.PermissionSettings
                            .FirstOrDefault(x => x.OptionUnitId == settingsDataContract.OptionUnitId);

                        if (settings != null) {
                            settings.IsAllow = settingsDataContract.IsAllow;

                            settingsRepository.UpdatePermissionSettings(
                                new PermissionSettings {
                                    Id = settings.Id,
                                    IsAllow = settings.IsAllow
                                });
                        } else {
                            var newEntity = settingsRepository
                                .AddPermissionSettings(
                                    new PermissionSettings {
                                        IsAllow = settingsDataContract.IsAllow,
                                        OptionGroupId = settingsDataContract.OptionGroupId,
                                        OptionUnitId = settingsDataContract.OptionUnitId,
                                        ProductPermissionSettingsId = productSettings.Id
                                    });

                            productSettings.PermissionSettings.Add(newEntity);
                        }
                    }

                    return productSettings;
                }
            });
    }
}
