using Dapper;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Repositories.Dealer.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Dealer {
    class DealerAccountMapProductPermissionSettingsRepository : IDealerAccountMapProductPermissionSettingsRepository {

        private IDbConnection _connection;

        public DealerAccountMapProductPermissionSettingsRepository(IDbConnection connection) {
            _connection = connection;
        }

        public long Add(DealerMapProductPermissionSettings dealerMapProductPermission) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT [DealerMapProductPermissionSettingsMap]([IsDeleted],[DealerAccountId],[ProductPermissionSettingsId]) " +
                "VALUES (0, @DealerAccountId, @ProductPermissionSettingsId);" +
                "SELECT SCOPE_IDENTITY()",
                dealerMapProductPermission);

        public DealerMapProductPermissionSettings Get(long mapId) =>
            _connection.QuerySingleOrDefault<DealerMapProductPermissionSettings>(
                "SELECT * FROM [DealerMapProductPermissionSettingsMap] " +
                "WHERE Id = @MapId",
                new { MapId = mapId });

        public DealerMapProductPermissionSettings Update(DealerMapProductPermissionSettings dealerMapProductPermission) {
            _connection.QuerySingleOrDefault<DealerMapProductPermissionSettings>(
                "UPDATE [DealerMapProductPermissionSettingsMap] " +
                "SET IsDeleted = @IsDeleted " +
                "WHERE Id = @Id;",
                dealerMapProductPermission);

            return Get(dealerMapProductPermission.Id);
        }
    }
}
