using MMSL.Domain.DataContracts.Types;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.CurrencyTypes;
using MMSL.Domain.Repositories.Types.Contracts;
using MMSL.Services.Types.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.Types {
    public class CurrencyTypeService : ICurrencyTypeService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ITypesRepositoriesFactory _typesRepositoriesFactory;

        public CurrencyTypeService(ITypesRepositoriesFactory typesRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _typesRepositoriesFactory = typesRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<CurrencyType> AddCurrencyTypeAsync(CurrencyTypeDataContract currencyType) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    CurrencyType currency = currencyType.GetEntity();

                    currency.Id = _typesRepositoriesFactory
                        .NewCurrencyTypeRepository(connection)
                        .AddCurrencyType(currency);

                    return currency;
                }
            });

        public Task<CurrencyType> DeleteCurrencyTypeAsync(long currencyTypeId) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    var repository = _typesRepositoriesFactory.NewCurrencyTypeRepository(connection);

                    CurrencyType currencyType = repository.GetCurrencyType(currencyTypeId);

                    currencyType.IsDeleted = true;

                    repository.UpdateCurrencyType(currencyType);

                    return currencyType;
                }
            });

        public Task<List<CurrencyType>> GetCurrencyTypesAsync() =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _typesRepositoriesFactory
                                .NewCurrencyTypeRepository(connection)
                                .GetCurrencyTypes();
                }
            });

        public Task<CurrencyType> UpdateCurrencyTypeAsync(CurrencyTypeDataContract currencyType) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _typesRepositoriesFactory
                        .NewCurrencyTypeRepository(connection)
                        .UpdateCurrencyType(currencyType.GetEntity());
                }
            });
    }
}
