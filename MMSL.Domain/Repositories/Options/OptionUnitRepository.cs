using Dapper;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Options {
    public class OptionUnitRepository : IOptionUnitRepository {

        private readonly IDbConnection _connection;

        public OptionUnitRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddOptionUnit(OptionUnit optionUnit) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [OptionUnits] " +
                "([IsDeleted], [OrderIndex], [Value], [ImageUrl], [ImageName], [IsMandatory], [OptionGroupId]) " +
                "VALUES (0, @OrderIndex, @Value, @ImageUrl, @ImageName, @IsMandatory, @OptionGroupId); " +
                "SELECT SCOPE_IDENTITY()",
                optionUnit);

        public OptionUnit GetOptionUnit(long optionUnitId) {
            OptionUnit result = null;

            _connection.Query<OptionUnit, UnitValue, OptionPrice, OptionUnit>(
                "SELECT [OptionUnits].*, [UnitValues].*, [OptionPrices].* " +
                "FROM [OptionUnits] " +
                "LEFT JOIN [UnitValues] ON [UnitValues].OptionUnitId = [OptionUnits].Id AND [UnitValues].IsDeleted = 0 " +
                "LEFT JOIN [OptionPrices] ON [OptionPrices].OptionUnitId = [OptionUnits].Id AND [OptionPrices].IsDeleted = 0 " +
                "WHERE [OptionUnits].Id = @Id " +
                "ORDER BY [UnitValues].OrderIndex ",
                (unit, unitValue, price) => {
                    if (result == null)
                        result = unit;

                    if (unitValue != null)
                        result.UnitValues.Add(unitValue);

                    if (price != null) {
                        result.CurrentPrice = price;
                    }

                    return unit;
                },
                new { Id = optionUnitId });

            return result;
        }

        public List<OptionUnit> GetOptionUnitsByGroup(long optionGroupId) {
            List<OptionUnit> result = new List<OptionUnit>();

            _connection.Query<OptionUnit, UnitValue, OptionPrice, OptionUnit>(
                "SELECT [OptionUnits].*, [UnitValues].* " +
                "FROM [OptionUnits] " +
                "LEFT JOIN [UnitValues] ON [UnitValues].OptionUnitId = [OptionUnits].Id " +
                "LEFT JOIN [OptionPrices] ON [OptionPrices].OptionUnitId = [OptionUnits].Id AND [OptionPrices].IsDeleted = 0 " +
                "WHERE [OptionUnits].[OptionGroupId] = @GroupId " +
                "AND [OptionUnits].IsDeleted = 0 " +
                "ORDER BY [OptionUnits].OrderIndex, [UnitValues].OrderIndex ",
                (unit, unitValue, price) => {
                    if (result.Any(x => x.Id == unit.Id))
                        unit = result.First(x => x.Id == unit.Id);
                    else
                        result.Add(unit);
                    
                    if (unitValue != null)
                        unit.UnitValues.Add(unitValue);

                    if (price != null) {
                        unit.CurrentPrice = price;
                    }

                    return unit;
                },
                new {
                    GroupId = optionGroupId
                });

            return result;
        }

        public void UpdateOptionUnit(OptionUnit optionUnit) =>
            _connection.Execute(
                "UPDATE [OptionUnits] " +
                "SET [IsDeleted]=@IsDeleted, [LastModified] = getutcdate(), " +
                "[Value] = @Value, [ImageUrl] = @ImageUrl, [ImageName] = @ImageName, [IsMandatory] = @IsMandatory, " +
                "[OrderIndex] = @OrderIndex " +
                "WHERE [OptionUnits].Id = @Id;",
                optionUnit);

        public void UpdateOptionUnitIndex(long optionUnitId, int orderIndex) =>
            _connection.Execute(
                "UPDATE [OptionUnits] " +
                "SET [LastModified] = getutcdate(), [OrderIndex] = @OrderIndex " +
                "WHERE [OptionUnits].Id = @Id;",
                new { Id = optionUnitId, OrderIndex = orderIndex });
    }
}
