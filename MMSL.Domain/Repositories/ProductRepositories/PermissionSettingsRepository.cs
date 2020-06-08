using Dapper;
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
            long settingId = _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [PermissionSettings] ([IsDeleted], [IsAllow], [ProductPermissionSettingsId], [OptionGroupId], [OptionUnitId]) " +
                "VALUES (0, @IsAllow, @ProductPermissionSettingsId, @OptionGroupId, @OptionUnitId);" +
                "SELECT SCOPE_IDENTITY()",
                settings);

            return GetPermissionSettingsById(settingId);
        }

        //public PermissionSettings GetPermissionSettings(long productSettingsId) {
        //    throw new System.NotImplementedException();
        //}

        public PermissionSettings GetPermissionSettingsById(long settingsId) =>
            _connection.QuerySingleOrDefault<PermissionSettings>(
                "SELECT [PermissionSettings].* " +
                "FROM [PermissionSettings] " +
                "WHERE [PermissionSettings].Id = @PermissionSettingId",
                new { PermissionSettingId = settingsId }
                );

        public PermissionSettings GetPermissionSettingsByOptionUnit(long id, long optionUnitId) =>
            _connection.QuerySingleOrDefault<PermissionSettings>(
                "SELECT [PermissionSettings].* " +
                "FROM [PermissionSettings] " +
                "WHERE [PermissionSettings].Id = @PermissionSettingId " +
                "AND [PermissionSettings].OptionUnitId = @UnitId",
                new { 
                    PermissionSettingId = id,
                    UnitId = optionUnitId
                });

        public PermissionSettings UpdatePermissionSettings(PermissionSettings settings) {
            _connection.Execute(
                "UPDATE [PermissionSettings] " +
                "SET [LastModified] = GETUTCDATE(), [IsDeleted] = @IsDeleted, [IsAllow] = @IsAllow " +
                "WHERE [PermissionSettings].Id = @Id",
                settings);

            return GetPermissionSettingsById(settings.Id);
        }
    }
}
