﻿using Dapper;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements {
    //TODO: update this
    public class MeasurementRepository : IMeasurementRepository {

        private readonly IDbConnection _connection;

        public MeasurementRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public List<Measurement> GetAll(string searchPhrase) {
            List<Measurement> result = new List<Measurement>();
            string query = "SELECT [Measurements].*, [MeasurementMapDefinitions].*, [MeasurementDefinitions].* " +
               "FROM [Measurements] " +
               "LEFT JOIN [MeasurementMapDefinitions] ON [MeasurementMapDefinitions].MeasurementId = [Measurements].Id " +
               "AND [MeasurementMapDefinitions].IsDeleted = 0 " +
               "LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [MeasurementMapDefinitions].MeasurementDefinitionId " +
               "WHERE [Measurements].IsDeleted = 0 " +
               "AND PATINDEX('%' + @SearchTerm + '%', [Measurements].[Name]) > 0 " +
               "AND [Measurements].ParentMeasurementId IS NULL";

            _connection.Query<Measurement, MeasurementMapDefinition, MeasurementDefinition, Measurement>(
               query,
               (measurement, measurementMapDefinition, measurementDefinition) => {

                   if (result.Any(x => x.Id == measurement.Id)) {
                       measurement = result.First(x => x.Id == measurement.Id);
                   } else {
                       result.Add(measurement);
                   }

                   if (measurementMapDefinition != null) {
                       measurementMapDefinition.MeasurementDefinition = measurementDefinition;

                       if (!measurement.MeasurementMapDefinitions.Any(x => x.MeasurementDefinitionId == measurementMapDefinition.MeasurementDefinitionId)) {
                           //MeasurementMapDefinition map = measurement.MeasurementMapDefinitions.First(x => x.MeasurementDefinitionId == measurementMapDefinition.MeasurementDefinitionId);
                           measurement.MeasurementMapDefinitions.Add(measurementMapDefinition);
                       }
                   }

                   return measurement;
               },
               new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase });

            return result;
        }

        public Measurement NewMeasurement(NewMeasurementDataContract newMeasurementDataContract) =>
            _connection.Query<Measurement>(
                "INSERT INTO [Measurements]([IsDeleted],[Name],[Description],[ParentMeasurementId]) " +
                "VALUES(0,@Name,@ProductCategoryId, @ParentMeasurementId)" +
                "SELECT * " +
                "FROM [Measurements]" +
                "WHERE [Measurements].Id = SCOPE_IDENTITY()",
                new {
                    Name = newMeasurementDataContract.Name,
                    ProductCategoryId = newMeasurementDataContract.ProductCategoryId,
                    ParentMeasurementId = newMeasurementDataContract.BaseMeasurementId
                })
            .SingleOrDefault();

        public void UpdateMeasurement(Measurement measurement) =>
            _connection.Execute(
                "UPDATE [Measurements] " +
                "SET [IsDeleted] = @IsDeleted, [LastModified]=getutcdate()," +
                "[Name]=@Name,[Description]=@Description " +
                "WHERE [Measurements].Id = @Id", measurement);

        public Measurement GetById(long measurementId) =>
            _connection.Query<Measurement>(
                  "SELECT * " +
                  "FROM [Measurements] " +
                  "WHERE Id = @Id",
                  new { Id = measurementId })
              .SingleOrDefault();

        //TODO: update this
        public Measurement GetByIdWithDependencies(long measurementId) {
            List<Measurement> measurementsResult = new List<Measurement>();

            IEnumerable<Measurement> middleResult = _connection.Query<Measurement, MeasurementMapDefinition, MeasurementDefinition, MeasurementSize, MeasurementMapValue, MeasurementDefinition, Measurement>(
               "SELECT [Measurements].*, " +
               "[MeasurementMapDefinitions].*, [MeasurementDefinitions].*, " +
               "[MeasurementSizes].*, [MeasurementValues].*, [ValueDefinition].*" +
               "FROM [Measurements] " +
               "LEFT JOIN [MeasurementMapDefinitions] ON [MeasurementMapDefinitions].MeasurementId = [Measurements].Id " +
               "AND [MeasurementMapDefinitions].IsDeleted = 0 " +
               "LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [MeasurementMapDefinitions].MeasurementDefinitionId " +
               "AND [MeasurementDefinitions].IsDeleted = 0 " +
               "LEFT JOIN [MeasurementSizes] ON [MeasurementSizes].MeasurementId = [Measurements].Id " +
               "AND [MeasurementSizes].IsDeleted = 0 " +
               "LEFT JOIN [MeasurementValues] ON [MeasurementValues].MeasurementSizeId = [MeasurementSizes].Id " +
               "AND [MeasurementValues].IsDeleted = 0 " +
               "LEFT JOIN [MeasurementDefinitions] AS [ValueDefinition] ON [ValueDefinition].Id = [MeasurementValues].MeasurementDefinitionId " +
               "AND [ValueDefinition].IsDeleted = 0 " +
               "WHERE [Measurements].Id = @Id AND [Measurements].IsDeleted = 0",
               (measurement, measurementMapDefinition, measurementDefinition, measurementSize, measurementValue, valueDefinition) => {

                   if (measurementsResult.Any(x => x.Id == measurement.Id)) {
                       measurement = measurementsResult.First(x => x.Id == measurement.Id);
                   } else {
                       measurementsResult.Add(measurement);
                   }

                   if (measurementMapDefinition != null) {
                       if (measurement.MeasurementMapDefinitions.Any(x => x.Id == measurementMapDefinition.Id)) {
                           measurementMapDefinition = measurement.MeasurementMapDefinitions.First(x => x.Id == measurementMapDefinition.Id);
                       } else {
                           measurement.MeasurementMapDefinitions.Add(measurementMapDefinition);
                       }

                       measurementMapDefinition.MeasurementDefinition = measurementDefinition;
                       measurement.MeasurementMapDefinitions.Add(measurementMapDefinition);
                   }

                   //if (measurementSize != null) {
                   //    if (measurement.MeasurementSizes.Any(x => x.Id == measurementSize.Id)) {
                   //        measurementSize = measurement.MeasurementSizes.First(x => x.Id == measurementSize.Id);
                   //    } else {
                   //        measurement.MeasurementSizes.Add(measurementSize);
                   //    }

                   //    if (measurementValue != null) {
                   //        if (measurementSize.Values.Any(x => x.Id == measurementValue.Id)) {
                   //            measurementValue = measurementSize.Values.First(x => x.Id == measurementValue.Id);
                   //        } else {
                   //            measurementSize.Values.Add(measurementValue);
                   //        }

                   //        measurementValue.MeasurementDefinition = valueDefinition;
                   //        measurementSize.Values.Add(measurementValue);
                   //    }

                   //}

                   return measurement;
               },
               new { Id = measurementId });

            return measurementsResult.FirstOrDefault();
        }

        public List<Measurement> GetAllByProduct(long productCategoryId) {
            List<Measurement> measurementsResult = new List<Measurement>();

            //_connection.Query<Measurement, MeasurementMapDefinition, MeasurementDefinition, MeasurementSize, MeasurementValue, MeasurementDefinition, Measurement>(
            //   "SELECT [Measurements].*, " +
            //   "[MeasurementMapDefinitions].*, [MeasurementDefinitions].*, " +
            //   "[MeasurementSizes].*, [MeasurementValues].*, [ValueDefinition].*" +
            //   "FROM [Measurements] " +
            //   "LEFT JOIN [MeasurementMapDefinitions] ON [MeasurementMapDefinitions].MeasurementId = [Measurements].Id " +
            //   "AND [MeasurementMapDefinitions].IsDeleted = 0 " +
            //   "LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [MeasurementMapDefinitions].MeasurementDefinitionId " +
            //   "AND [MeasurementDefinitions].IsDeleted = 0 " +
            //   "LEFT JOIN [MeasurementSizes] ON [MeasurementSizes].MeasurementId = [Measurements].Id " +
            //   "AND [MeasurementSizes].IsDeleted = 0 " +
            //   "LEFT JOIN [MeasurementValues] ON [MeasurementValues].MeasurementSizeId = [MeasurementSizes].Id " +
            //   "AND [MeasurementValues].IsDeleted = 0 " +
            //   "LEFT JOIN [MeasurementDefinitions] AS [ValueDefinition] ON [ValueDefinition].Id = [MeasurementValues].MeasurementDefinitionId " +
            //   "AND [ValueDefinition].IsDeleted = 0 " +
            //   "WHERE [Measurements].ProductCategoryId = @ProductCategoryId AND [Measurements].IsDeleted = 0",
            //   (measurement, measurementMapDefinition, measurementDefinition, measurementSize, measurementValue, valueDefinition) => {

            //       if (measurementsResult.Any(x => x.Id == measurement.Id)) {
            //           measurement = measurementsResult.First(x => x.Id == measurement.Id);
            //       } else {
            //           measurementsResult.Add(measurement);
            //       }

            //       if (measurementMapDefinition != null) {
            //           if (measurement.MeasurementMapDefinitions.Any(x => x.Id == measurementMapDefinition.Id)) {
            //               measurementMapDefinition = measurement.MeasurementMapDefinitions.First(x => x.Id == measurementMapDefinition.Id);
            //           } else {
            //               measurement.MeasurementMapDefinitions.Add(measurementMapDefinition);
            //           }

            //           measurementMapDefinition.MeasurementDefinition = measurementDefinition;
            //           measurement.MeasurementMapDefinitions.Add(measurementMapDefinition);
            //       }

            //       if (measurementSize != null) {
            //           if (measurement.MeasurementSizes.Any(x => x.Id == measurementSize.Id)) {
            //               measurementSize = measurement.MeasurementSizes.First(x => x.Id == measurementSize.Id);
            //           } else {
            //               measurement.MeasurementSizes.Add(measurementSize);
            //           }

            //           if (measurementValue != null) {
            //               if (measurementSize.Values.Any(x => x.Id == measurementValue.Id)) {
            //                   measurementValue = measurementSize.Values.First(x => x.Id == measurementValue.Id);
            //               } else {
            //                   measurementSize.Values.Add(measurementValue);
            //               }

            //               measurementValue.MeasurementDefinition = valueDefinition;
            //               measurementSize.Values.Add(measurementValue);
            //           }

            //       }

            //       return measurement;
            //   },
            //   new { ProductCategoryId = productCategoryId });

            return measurementsResult;
        }

        public Measurement GetByIdWithDefinitions(long measurementId) {
            Measurement result = null;

            string query = "SELECT [Measurements].*, [MeasurementMapDefinitions].*, [MeasurementDefinitions].* " +
                "FROM [Measurements] " +
                "LEFT JOIN [MeasurementMapDefinitions] ON [MeasurementMapDefinitions].MeasurementId = [Measurements].Id " +
                "AND [MeasurementMapDefinitions].IsDeleted = 0 " +
                "LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [MeasurementMapDefinitions].MeasurementDefinitionId " +
                "WHERE [Measurements].Id = @Id AND [Measurements].IsDeleted = 0";

            _connection.Query<Measurement, MeasurementMapDefinition, MeasurementDefinition, Measurement>(
                query,
                (measurement, measurementMapDefinition, measurementDefinition) => {
                    if (result == null) {
                        result = measurement;
                    }

                    if (measurementMapDefinition != null) {
                        measurementMapDefinition.MeasurementDefinition = measurementDefinition;

                        result.MeasurementMapDefinitions.Add(measurementMapDefinition);
                    }

                    return measurement;
                },
                new { Id = measurementId });

            return result;
        }

        public List<MeasurementMapSize> GetSizesByMeasurementId(long measurementId, long? parentMeasurementId) {
            List<MeasurementMapSize> results = new List<MeasurementMapSize>();

            string query =
                "SELECT [MeasurementMapSizes].*, " +
                "[MeasurementSizes].*, " +
                "[MeasurementMapValues].*, " +
                "[MeasurementDefinitions].* " +
                "FROM [MeasurementMapSizes] " +
                "LEFT JOIN [MeasurementSizes] ON [MeasurementMapSizes].MeasurementSizeId = [MeasurementSizes].Id " +
                "LEFT JOIN [MeasurementMapValues] ON [MeasurementMapValues].MeasurementSizeId = [MeasurementSizes].Id " +
                "LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [MeasurementMapValues].MeasurementDefinitionId " +
                "WHERE [MeasurementMapSizes].MeasurementId = @Id " +
                (parentMeasurementId.HasValue ? "OR [MeasurementMapSizes].MeasurementId = @ParentId " : string.Empty) +
                "AND [MeasurementMapSizes].IsDeleted = 0";

            _connection.Query<MeasurementMapSize, MeasurementSize, MeasurementMapValue, MeasurementDefinition, MeasurementMapSize>(
                query,
                (mapSize, size, value, definition) => {

                    if (results.Any(x => x.Id.Equals(mapSize.Id))) {
                        mapSize = results.First(x => x.Id.Equals(mapSize.Id));
                        size = mapSize.MeasurementSize;
                    } else {
                        results.Add(mapSize);
                        mapSize.MeasurementSize = size;
                    }

                    if (value != null) {
                        value.MeasurementDefinition = definition;

                        size.MeasurementMapValues.Add(value);
                    }

                    return mapSize;
                },
                new {
                    Id = measurementId,
                    ParentId = parentMeasurementId
                });

            return results;
        }
    }
}
