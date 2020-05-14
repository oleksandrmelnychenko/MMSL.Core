using Dapper;
using MMSL.Domain.Entities.CurrencyTypes;
using MMSL.Domain.Repositories.Types.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Types {
    class CurrencyTypeRepository : ICurrencyTypeRepository {

        private IDbConnection _connection;

        public CurrencyTypeRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddCurrencyType(CurrencyType currencyType) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [CurrencyTypes] " +
                "([IsDeleted], [Created], [Name], [Description]) " +
                "VALUES (0, @Created, @Name, @Description);" +
                "SELECT SCOPE_IDENTITY()",
                currencyType);

        public CurrencyType GetCurrencyType(long id) =>
            _connection.QuerySingleOrDefault<CurrencyType>(
                "SELECT * FROM [CurrencyTypes] " +
                "WHERE [CurrencyTypes].Id = @Id",
                new { Id = id });

        public List<CurrencyType> GetCurrencyTypes() =>
            _connection.Query<CurrencyType>("SELECT * FROM [CurrencyTypes]").ToList();

        public CurrencyType UpdateCurrencyType(CurrencyType currencyType) =>
            _connection.QuerySingleOrDefault<CurrencyType>(
                "UPDATE [CurrencyTypes] SET " +
                "[IsDeleted] = @IsDeleted, [LastModified]=getutcdate(), " +
                "[Name] = @Name, [Description] = @Description " +
                "WHERE [CurrencyTypes].Id = @Id",
                currencyType);
    }
}
