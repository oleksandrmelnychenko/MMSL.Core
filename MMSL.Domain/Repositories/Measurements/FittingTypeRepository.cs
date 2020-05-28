﻿using Dapper;
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

        public List<FittingType> GetAll(string searchPhrase) =>
            _connection.Query<FittingType>(
                "SELECT * " +
                "FROM [FittingTypes] " +
                "WHERE [FittingTypes].Id = 0 AND PATINDEX('%' + @SearchTerm + '%', [FittingTypes].Type) > 0 ",
                new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase })
            .ToList();

        public FittingType Add(string type, string unit, long dealerAccountId) =>
            _connection.QuerySingleOrDefault<FittingType>(
                "INSERT INTO [FittingTypes]([IsDeleted],[Type],[Unit],[DealerAccountId]) " +
                "VALUES (0,@Type,@Unit,@DealerAccountId) " +
                "SELECT * " +
                "FROM [FittingTypes] " +
                "WHERE [FittingTypes].Id = SCOPE_IDENTITY()",
                new {
                    Type = type,
                    Unit = unit,
                    DealerAccountId = dealerAccountId
                });
    }
}