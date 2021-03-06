﻿using Dapper;
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

        public MeasurementSize AddMeasurementSize(string name, string description) {
            return _connection.QuerySingleOrDefault<MeasurementSize>(
                    "INSERT INTO [MeasurementSizes] ([IsDeleted],[Name],[Description] ) " +
                    "VALUES (0, @Name, @Description) " +
                    "SELECT * " +
                    "FROM [MeasurementSizes] " +
                    "WHERE [MeasurementSizes].Id = SCOPE_IDENTITY()", new { Name = name, Description = description }
                    );
        }

        public MeasurementSize GetById(long measurementSizeId) {
            MeasurementSize result = null;

            _connection.Query<MeasurementSize, MeasurementMapValue, MeasurementSize>(
                    "SELECT s.*, [MeasurementMapValues].* " +
                    "FROM [MeasurementSizes] AS s " +
                    "LEFT JOIN [MeasurementMapValues] ON [MeasurementMapValues].MeasurementSizeId = s.Id " +
                    "WHERE s.Id = @MeasurementSizeId AND s.IsDeleted = 0",
                    (size, value) => {
                        if (result == null) {
                            result = size;
                        }

                        if (value != null)
                            result.MeasurementMapValues.Add(value);

                        return size;
                    },
                    new {
                        MeasurementSizeId = measurementSizeId
                    });

            return result;
        }

        public List<MeasurementSize> GetAllByMeasurementId(long measurementId) {
            return _connection.Query<MeasurementSize>(
                    "SELECT s.* " +
                    "FROM [MeasurementSizes] as s " +
                    "LEFT JOIN [MeasurementMapSizes] AS ms ON ms.MeasurementSizeId = s.Id" +
                    " AND ms.IsDeleted = 0 " +
                    "LEFT JOIN [Measurements] as m ON m.Id = ms.MeasurementId" +
                    " AND m.IsDeleted = 0 " +
                    "WHERE s.IsDeleted = 0 AND m.Id = @Id " +
                    "ORDER BY s.Name",
                    new {
                        Id = measurementId
                    }).ToList();
        }

        public void UpdateMeasurementSize(MeasurementSize measurementSize) =>
             _connection.Execute(
                    "UPDATE [MeasurementSizes] SET " +
                    "[IsDeleted] = @IsDeleted, [LastModified] = GETUTCDATE(), [Name] = @Name, " +
                    "[Description] = @Description " +
                    "WHERE [MeasurementSizes].Id = @Id", measurementSize);
    }
}

