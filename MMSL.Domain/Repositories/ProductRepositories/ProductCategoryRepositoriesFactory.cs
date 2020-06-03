using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.ProductRepositories {
    public class ProductCategoryRepositoriesFactory : IProductCategoryRepositoriesFactory {
        public IProductCategoryRepository NewProductCategoryRepository(IDbConnection connection) =>
            new ProductCategoryRepository(connection);

        public IProductCategoryMapOptionGroupsRepository NewProductCategoryMapOptionGroupsRepository(IDbConnection connection) =>
            new ProductCategoryMapOptionGroupsRepository(connection);

        public IProductPermissionSettingsRepository NewProductPermissionSettingsRepository(IDbConnection connection) =>
            new ProductPermissionSettingsRepository(connection);

        public IPermissionSettingsRepository NewPermissionSettingsRepository(IDbConnection connection) =>
            new PermissionSettingsRepository(connection);
    }
}
