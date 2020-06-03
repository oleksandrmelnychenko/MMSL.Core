using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.ProductRepositories {
    class ProductPermissionSettingsRepository : IProductPermissionSettingsRepository {

        private readonly IDbConnection _connection;

        public ProductPermissionSettingsRepository(IDbConnection connection) {
            _connection = connection;
        }

        public ProductPermissionSettings AddProductPermissionSettings(ProductPermissionSettings productSettings) {
            throw new System.NotImplementedException();
        }

        public ProductPermissionSettings GetProductPermissionSettings(long productId) {
            throw new System.NotImplementedException();
        }

        public ProductPermissionSettings GetProductPermissionSettingsById(long productSettingsId) {
            throw new System.NotImplementedException();
        }

        public ProductPermissionSettings UpdateProductPermissionSettings(long productSettingsId) {
            throw new System.NotImplementedException();
        }
    }
}
