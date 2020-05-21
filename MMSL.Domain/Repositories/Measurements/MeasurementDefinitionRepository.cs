using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
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

    }
}
