using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementMapDefinitionRepository : IMeasurementMapDefinitionRepository {


        private readonly IDbConnection _connection;

        public MeasurementMapDefinitionRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddMeasurementMapDefinition(MeasurementMapDefinition measurementMapDefinition) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [MeasurementMapDefinitions] " +
                "([IsDeleted], [MeasurementId], [MeasurementDefinitionId]) " +
                "VALUES (0, @MeasurementId, @MeasurementDefinitionId)" +
                "SELECT SCOPE_IDENTITY()",
                measurementMapDefinition);

        public MeasurementMapDefinition GetMeasurementMapDefinition(long measurementMapDefinitionId) =>
            _connection.QuerySingleOrDefault<MeasurementMapDefinition>(
                "SELECT * FROM [MeasurementMapDefinitions] " +
                "WHERE [MeasurementMapDefinitions].Id = @Id",
                new { Id = measurementMapDefinitionId });

        public MeasurementMapDefinition UpdateMeasurementMapDefinition(MeasurementMapDefinition measurementMapDefinition) =>
            _connection.QuerySingleOrDefault<MeasurementMapDefinition>(
                "UPDATE [MeasurementMapDefinitions] " +
                "SET [IsDeleted] = @IsDeleted," +
                "[MeasurementId] = @MeasurementId," +
                "[MeasurementDefinitionId] = @MeasurementDefinitionId " +
                "WHERE [MeasurementMapDefinitions].Id = @Id",
                measurementMapDefinition);
    }
}
