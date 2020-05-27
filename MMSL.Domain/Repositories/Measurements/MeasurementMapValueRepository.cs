using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Measurements {
    //TODO: update this
    public class MeasurementMapValueRepository : IMeasurementMapValueRepository {

        private readonly IDbConnection _connection;

        public MeasurementMapValueRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddValue(MeasurementMapValue value) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [MeasurementMapValues] " +
                "([IsDeleted], [Value], [MeasurementId], [MeasurementDefinitionId], [MeasurementSizeId]) " +
                "VALUES (0, @Value, @MeasurementId, @MeasurementDefinitionId, @MeasurementSizeId);" +
                "SELECT SCOPE_IDENTITY()",
                value);

        public MeasurementMapValue UpdateValue(MeasurementMapValue measurementValue) =>
            _connection.QuerySingleOrDefault<MeasurementMapValue>(
                "UPDATE [MeasurementMapValues] " +
                "SET [IsDeleted] = @IsDeleted, [LastModified] = GETUTCDATE(), [Value] = @Value " +
                "WHERE [MeasurementMapValues].Id = @Id;" +
                "SELECT * FROM [MeasurementMapValues] " +
                "WHERE [MeasurementMapValues].Id = @Id;",
                measurementValue);
    }
}
