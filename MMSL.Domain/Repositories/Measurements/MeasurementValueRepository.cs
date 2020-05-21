using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementValueRepository : IMeasurementValueRepository {

        private readonly IDbConnection _connection;

        public MeasurementValueRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddValue(MeasurementValue value) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [MeasurementValues] " +
                "([IsDeleted], [Value], [MeasurementDefinitionId], [MeasurementSizeId]) " +
                "VALUES (0, @Value, @MeasurementDefinitionId, @MeasurementSizeId);" +
                "SELECT SCOPE_IDENTITY()",
                value);

        public MeasurementValue UpdateValue(MeasurementValue measurementValue) =>
            _connection.QuerySingleOrDefault<MeasurementValue>(
                "UPDATE [MeasurementValues] " +
                "SET [IsDeleted] = @IsDeleted, [LastModified] = GETUTCDATE(), [Value] = @Value " +
                "WHERE [MeasurementValues].Id = @Id;" +
                "SELECT * FROM [MeasurementValues] " +
                "WHERE [MeasurementValues].Id = @Id;",
                measurementValue);
    }
}
