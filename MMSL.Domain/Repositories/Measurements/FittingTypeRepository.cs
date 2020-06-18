using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Domain.Repositories.Measurements {
    public class FittingTypeRepository : IFittingTypeRepository {

        private readonly IDbConnection _connection;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connection"></param>
        public FittingTypeRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<FittingType> GetAll(string searchPhrase, long measurementId) =>
            _connection.Query<FittingType>(
                "SELECT * " +
                "FROM [FittingTypes] " +
                "WHERE [FittingTypes].IsDeleted = 0 AND PATINDEX('%' + @SearchTerm + '%', [FittingTypes].Type) > 0 " +
                "AND [MeasurementId] = @MeasurementId",
                new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase, MeasurementId = measurementId })
            .ToList();

        public FittingType GetById(long fittingTypeId) =>
            _connection.Query<FittingType, MeasurementMapValue, MeasurementDefinition, FittingType>(
                "SELECT ft.*, mv.*, d.* " +
                "FROM [FittingTypes] AS ft " +
                "LEFT JOIN [MeasurementMapValues] AS mv ON mv.FittingTypeId = ft.Id " +
                "LEFT JOIN [MeasurementDefinitions] AS d ON d.Id = mv.MeasurementDefinitionId " +
                "WHERE ft.IsDeleted = 0 AND ft.Id = @Id",
                (fittingType, measurementMapValue, measurementDefinition) => {
                    if (measurementMapValue != null) {
                        fittingType.MeasurementMapValues.Add(measurementMapValue);

                        if (measurementDefinition != null) {
                            measurementMapValue.MeasurementDefinition = measurementDefinition;
                        }
                    }
                    return fittingType;
                },
                new { Id = fittingTypeId })
            .SingleOrDefault();

        public FittingType Add(string type, string unit, long dealerAccountId, long measurementId) =>
            _connection.QuerySingleOrDefault<FittingType>(
                "INSERT INTO [FittingTypes]([IsDeleted],[Type],[Unit],[DealerAccountId],[MeasurementId]) " +
                "VALUES (0,@Type,@Unit,@DealerAccountId,@MeasurementId) " +
                "SELECT * " +
                "FROM [FittingTypes] " +
                "WHERE [FittingTypes].Id = SCOPE_IDENTITY()",
                new {
                    Type = type,
                    Unit = unit,
                    DealerAccountId = dealerAccountId,
                    MeasurementId = measurementId
                });

        public void Update(FittingType fittingType) =>
            _connection.Execute(
                "UPDATE [FittingTypes] " +
                "SET [IsDeleted]=@IsDeleted,[Type]=@Type,[Unit]=@Unit,[LastModified]=getutcdate() " +
                "WHERE [FittingTypes].Id = @Id", fittingType);
    }
}
