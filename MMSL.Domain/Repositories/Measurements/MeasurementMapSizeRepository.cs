using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementMapSizeRepository : IMeasurementMapSizeRepository {

        private readonly IDbConnection _connection;

        public MeasurementMapSizeRepository(IDbConnection connection) {
            _connection = connection;
        }

        public MeasurementMapSize New(long measurementId, long measurementSizeId) =>
            _connection.Query<MeasurementMapSize>(
                "INSERT INTO [MeasurementMapSizes] ([IsDeleted],[MeasurementId],[MeasurementSizeId]) " +
                "VALUES (0, @MeasurementId, @MeasurementSizeId) " +
                "SELECT * " +
                "FROM [MeasurementMapSizes]" +
                "WHERE [MeasurementMapSizes].Id = SCOPE_IDENTITY()",
                new {
                    MeasurementId = measurementId,
                    MeasurementSizeId = measurementSizeId
                })
            .SingleOrDefault();

        public void Update(MeasurementMapSize measurementMapSize) =>
            _connection.Execute(
                "UPDATE [MeasurementMapSizes] " +
                "SET [IsDeleted] = @IsDeleted, [LastModified]=getutcdate() " +
                "WHERE [MeasurementMapSizes].Id = @Id", measurementMapSize);
    }
}
