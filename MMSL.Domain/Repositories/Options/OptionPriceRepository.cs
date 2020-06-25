using Dapper;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Options {
    class OptionPriceRepository : IOptionPriceRepository {

        private readonly IDbConnection _connection;

        public OptionPriceRepository(IDbConnection connection) {
            _connection = connection;
        }

        public OptionPrice AddPrice(OptionPrice price) =>
            _connection.QuerySingleOrDefault<OptionPrice>(
                "INSERT INTO [OptionPrices] (IsDeleted, Price, CurrencyTypeId, OptionGroupId, OptionUnitId) " +
                "VALUES(0, @Price, @CurrencyTypeId, @OptionGroupId, @OptionUnitId)",
                price);

        public OptionPrice GetPrice(long priceId) =>
            _connection.QuerySingleOrDefault<OptionPrice>(
                "SELECT [OptionPrices].* " +
                "FROM [OptionPrices] " +
                "WHERE [OptionPrices].Id = @PriceId",
                new { PriceId = priceId });

        public List<OptionPrice> GetPrices(long? optionUnitId, long? optionGroupId) =>
            _connection.Query<OptionPrice>(
                @"SELECT [OptionPrices].* FROM[OptionPrices]" +
                (optionGroupId.HasValue ? "WHERE[OptionPrices].OptionGroupId = @OptionGroupId " : string.Empty) +
                (optionUnitId.HasValue ? "WHERE [OptionPrices].OptionUnitId = @OptionUnitId " : string.Empty),
                new {
                    OptionGroupId = optionGroupId,
                    OptionUnitId = optionUnitId
                }).ToList();

        public OptionPrice UpdatePrice(OptionPrice price) =>
            _connection.QuerySingleOrDefault<OptionPrice>(
                "UPDATE [OptionPrices] " +
                "SET [IsDeleted] = @IsDeleted," +
                "[LastModified] = GETUTCDATE()," +
                "[Price]=@Price," +
                "[CurrencyTypeId]=@CurrencyTypeId",
                price);
    }
}
