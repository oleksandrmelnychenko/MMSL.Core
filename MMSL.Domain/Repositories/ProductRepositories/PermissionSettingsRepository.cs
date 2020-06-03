using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.ProductRepositories {
    class PermissionSettingsRepository : IPermissionSettingsRepository {
        private readonly IDbConnection _connection;

        public PermissionSettingsRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public PermissionSettings AddPermissionSettings(PermissionSettings settings) {
            throw new System.NotImplementedException();
        }

        public PermissionSettings GetPermissionSettings(long productSettingsId) {
            throw new System.NotImplementedException();
        }

        public PermissionSettings GetPermissionSettingsById(long settingsId) {
            throw new System.NotImplementedException();
        }

        public PermissionSettings UpdatePermissionSettings(long settingsId) {
            throw new System.NotImplementedException();
        }
    }
}
