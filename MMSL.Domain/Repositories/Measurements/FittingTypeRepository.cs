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

        private const string _mappedQuery =
@"SELECT [FittingTypes].*, [Measurements].*, [MeasurementUnits].*, [MeasurementMapValues].*, [MeasurementDefinitions].* 
FROM [FittingTypes] 
LEFT JOIN [Measurements] ON [Measurements].Id = [FittingTypes].MeasurementId AND [Measurements].IsDeleted = 0 
LEFT JOIN [MeasurementUnits] ON [MeasurementUnits].Id = [Measurements].MeasurementUnitId 
LEFT JOIN [MeasurementMapValues] ON [MeasurementMapValues].FittingTypeId = [FittingTypes].Id AND [MeasurementMapValues].IsDeleted = 0 
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [MeasurementMapValues].MeasurementDefinitionId 
WHERE [FittingTypes].IsDeleted = 0 ";

        private readonly IDbConnection _connection;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connection"></param>
        public FittingTypeRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<FittingType> GetAll(string searchPhrase, long measurementId) {
            List<FittingType> results = new List<FittingType>();

            _connection.Query<FittingType, Measurement, MeasurementUnit, MeasurementMapValue, MeasurementDefinition, FittingType>(
                _mappedQuery +
                "AND PATINDEX('%' + @SearchTerm + '%', [FittingTypes].Type) > 0 " +
                "AND [FittingTypes].MeasurementId = @MeasurementId",
                (fitType, measurement, unit, value, definition) => {
                    if (results.Any(x => x.Id == fitType.Id)) {
                        fitType = results.First(x => x.Id == fitType.Id);
                    } else {
                        fitType.Measurement = measurement;
                        measurement.MeasurementUnit = unit;
                        results.Add(fitType);
                    }

                    if (value != null) {
                        value.MeasurementDefinition = definition;
                        fitType.MeasurementMapValues.Add(value);
                    }

                    return fitType;
                },
                new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase, MeasurementId = measurementId });

            return results;
        }

        public FittingType GetById(long fittingTypeId) {
            FittingType result = null;

            _connection.Query<FittingType, Measurement, MeasurementUnit, MeasurementMapValue, MeasurementDefinition, FittingType>(
                _mappedQuery + "AND [FittingTypes].Id = @Id",
                (fittingType, measurement, unit, measurementMapValue, measurementDefinition) => {
                    fittingType.Measurement = measurement;
                    measurement.MeasurementUnit = unit;

                    if (result == null)
                        result = fittingType;

                    if (measurementMapValue != null) {
                        result.MeasurementMapValues.Add(measurementMapValue);

                        if (measurementDefinition != null) {
                            measurementMapValue.MeasurementDefinition = measurementDefinition;
                        }
                    }

                    return fittingType;
                },
                new { Id = fittingTypeId });

            return result;
        }

        public FittingType Add(string type, long measurementUnitId, long measurementId) {
            FittingType result = null;

            _connection.Query<FittingType, Measurement, MeasurementUnit, MeasurementMapValue, MeasurementDefinition, FittingType>(
                "INSERT INTO [FittingTypes]([IsDeleted],[Type],[MeasurementId]) " +
                "VALUES (0,@Type,@MeasurementId) " +
                _mappedQuery +
                "AND [FittingTypes].Id = SCOPE_IDENTITY()",
                (fittingType, measurement, unit, measurementMapValue, measurementDefinition) => {
                    fittingType.Measurement = measurement;
                    measurement.MeasurementUnit = unit;

                    if (result == null)
                        result = fittingType;

                    if (measurementMapValue != null) {
                        result.MeasurementMapValues.Add(measurementMapValue);

                        if (measurementDefinition != null) {
                            measurementMapValue.MeasurementDefinition = measurementDefinition;
                        }
                    }

                    return fittingType;
                },
                new {
                    Type = type,
                    MeasurementUnitId = measurementUnitId,
                    MeasurementId = measurementId
                });

            return result;
        }

        public void Update(FittingType fittingType) =>
            _connection.Execute(
                "UPDATE [FittingTypes] " +
                "SET [IsDeleted]=@IsDeleted,[Type]=@Type,[LastModified]=getutcdate() " +
                "WHERE [FittingTypes].Id = @Id", fittingType);
    }
}
