using Dapper;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Options {
    public class OptionUnitRepository : IOptionUnitRepository {

        private IDbConnection _connection;

        public OptionUnitRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddOptionUnit(OptionUnit optionUnit) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [OptionUnits] " +
                "([IsDeleted], [Name], [Value], [ImageUrl], [OptionGroupId]) " +
                "VALUES (0, @Name, @Value, @ImageUrl, @OptionGroupId); " +
                "SELECT SCOPE_IDENTITY()",
                optionUnit);

        public OptionUnit GetOptionUnit(long optionUnitId) =>
            _connection.QuerySingleOrDefault<OptionUnit>(
                "SELECT * FROM [OptionUnits] " +
                "WHERE [OptionUnits].Id = @Id",
                new { Id = optionUnitId });

        public List<OptionUnit> GetOptionUnitsByGroup(long optionGroupId) =>
            _connection.Query<OptionUnit>(
                "SELECT * FROM [OptionUnits] " +
                "WHERE [OptionUnits].[OptionGroupId] = @GroupId",
                new {
                    GroupId = optionGroupId
                }).ToList();

        public void UpdateOptionUnit(OptionUnit optionUnit) =>
            _connection.Execute(
                "UPDATE [OptionUnits] " +
                "SET [IsDeleted]=@IsDeleted, [LastModified] = getutcdate(), [Name] = @Name, " +
                "[Value] = @Value, [ImageUrl] = @ImageUrl, [OptionGroupId] = @OptionGroupId " +
                "WHERE [OptionUnits].Id = @Id;",
                optionUnit);
    }
}
