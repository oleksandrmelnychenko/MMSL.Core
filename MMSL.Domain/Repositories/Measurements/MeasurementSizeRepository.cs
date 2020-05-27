using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementSizeRepository : IMeasurementSizeRepository {

        private readonly IDbConnection _connection;

        public MeasurementSizeRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddMeasurementSize(MeasurementSize measurementSize) {
            return _connection.QuerySingleOrDefault<long>(
                    "INSERT INTO [MeasurementSizes] " +
                    "([IsDeleted],[Name],[Description],[MeasurementId]) " +
                    "VALUES (0, @Name, @Description, @MeasurementId) " +
                    "SELECT SCOPE_IDENTITY()",
                    measurementSize
                    );
        }

        public MeasurementSize GetMeasurementSize(long measurementSizeId) {
            return _connection.QuerySingleOrDefault<MeasurementSize>(
                    "SELECT [MeasurementSizes].*, [MeasurementValues].*, [MeasurementDefinitions].* " +
                    "FROM [MeasurementSizes] " +
                    "LEFT JOIN [MeasurementValues] ON [MeasurementValues].MeasurementSizeId = [MeasurementSizes].Id " +
                    "LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [MeasurementValues].MeasurementDefinitionId " +
                    "WHERE [MeasurementSizes].Id = @MeasurementSizeId " +
                    "AND [MeasurementSizes].IsDeleted = 0",
                    new {
                        MeasurementSizeId = measurementSizeId
                    });
        }

        public List<MeasurementSize> GetSizesByMeasurementId(long measurementId) {
            return _connection.Query<MeasurementSize>(
                    "SELECT * " +
                    "FROM [MeasurementSizes] as s " +
                    "LEFT JOIN [MeasurementMapSizes] AS ms ON ms.MeasurementSizeId = s.Id " +
                    "LEFT JOIN [Measurements] as m ON m.Id = ms.MeasurementId " +
                    "WHERE m.Id = @Id " +
                    "ORDER BY s.Name",
                    new {
                        Id = measurementId
                    }
                    ).ToList();
        }

        public MeasurementSize UpdateMeasurementSize(MeasurementSize measurementSize) {
            return _connection.QuerySingleOrDefault<MeasurementSize>(
                    "UPDATE [MeasurementSizes] SET " +
                    "[IsDeleted] = @IsDeleted, [LastModified] = GETUTCDATE(), [Name] = @Name, " +
                    "[Description] = @Description, [MeasurementId] = @MeasurementId " +
                    "WHERE [MeasurementSizes].Id = @Id " +
                    "SELECT TOP(1) [MS].* FROM [MeasurementSizes] AS [MS] WHERE [MS].Id = SCOPE_IDENTITY()",
                    measurementSize
                    );
        }
    }
}
