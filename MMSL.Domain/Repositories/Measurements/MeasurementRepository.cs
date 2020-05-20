using Dapper;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements {
    public class MeasurementRepository : IMeasurementRepository {

        private readonly IDbConnection _connection;

        public MeasurementRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public List<Measurement> GetAll(string searchPhrase) =>
             _connection.Query<Measurement>(
                "SELECT *" +
                "FROM [Measurements] " +
                "WHERE IsDeleted = 0 AND PATINDEX('%' + @SearchTerm + '%', [Measurements].Name) > 0",
                new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase })
            .ToList();

        public Measurement NewMeasurement(NewMeasurementDataContract newMeasurementDataContract) =>
            _connection.Query<Measurement>(
                "INSERT INTO [Measurements]([IsDeleted],[Name],[Description],[ProductCategoryId]) " +
                "VALUES(0,@Name,@Description,@ProductCategoryId)" +
                "SELECT * " +
                "FROM [Measurements]" +
                "WHERE [Measurements].Id = SCOPE_IDENTITY()",
                new {
                    Name = newMeasurementDataContract.Name,
                    ProductCategoryId = newMeasurementDataContract.ProductCategoryId,
                    Description = newMeasurementDataContract.Description
                })
            .SingleOrDefault();
    }
}
