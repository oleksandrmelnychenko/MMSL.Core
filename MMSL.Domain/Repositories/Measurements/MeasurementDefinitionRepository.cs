using Dapper;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementDefinitionRepository : IMeasurementDefinitionRepository {

        private readonly IDbConnection _connection;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connection"></param>
        public MeasurementDefinitionRepository(IDbConnection connection) {
            _connection = connection;
        }

        public int CountDefinitionReferences(long measurementDefinitionId) =>
            _connection.QuerySingleOrDefault("SELECT COUNT(DISTINCT MeasurementId) " +
                "FROM [MeasurementMapDefinitions] " +
                "WHERE IsDeleted = 0 " +
                "AND MeasurementDefinitionId = @Id",
                new { Id = measurementDefinitionId });

        public List<MeasurementDefinition> GetAll(string searchPhrase, bool? isDefault) {
            string query = 
                "SELECT * " +
                "FROM [MeasurementDefinitions] " +
                "WHERE [IsDeleted] = 0 AND PATINDEX('%' + @SearchTerm + '%', [MeasurementDefinitions].Name) > 0 ";

            if (isDefault.HasValue) {
                query += "AND [IsDefault] = @IsDefault";
            }

            return _connection.Query<MeasurementDefinition>(
                query,
                new {
                    SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase,
                    IsDefault = isDefault
                }).ToList();
        }

        public MeasurementDefinition GetById(long measurementDefinitionId) =>
            _connection.Query<MeasurementDefinition>(
                "SELECT * " +
                "FROM [MeasurementDefinitions] " +
                "WHERE Id = @Id", new { Id = measurementDefinitionId })
            .SingleOrDefault();

        public MeasurementDefinition NewMeasurementDefinition(MeasurementDefinition newMeasurementDefinitionDataContract) =>
            _connection.Query<MeasurementDefinition>(
                "INSERT INTO [MeasurementDefinitions]([IsDeleted],[Name],[Description],[IsDefault]) " +
                "VALUES(0,@Name,@Description,@IsDefault) " +
                "SELECT *" +
                "FROM [MeasurementDefinitions] " +
                "WHERE [MeasurementDefinitions].Id = SCOPE_IDENTITY()", newMeasurementDefinitionDataContract)
            .SingleOrDefault();

        public void UpdateMeasurementDefinition(MeasurementDefinition measurementDefinition) =>
            _connection.Execute(
                "UPDATE [MeasurementDefinitions] " +
                "SET [IsDeleted] = @IsDeleted,[Name]=@Name,[Description]=@Description,[LastModified]=getutcdate(),[IsDefault]=@IsDefault " +
                "WHERE [MeasurementDefinitions].Id = @Id", measurementDefinition);
    }
}
