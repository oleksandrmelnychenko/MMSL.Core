using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using MMSL.Services.OptionServices.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices {
    public class OptionUnitService : IOptionUnitService {

        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IOptionRepositoriesFactory _optionRepositoriesFactory;

        public OptionUnitService(IOptionRepositoriesFactory optionRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _optionRepositoriesFactory = optionRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<OptionUnit> GetOptionUnitByIdAsync(long optionUnitId) =>
            Task.Factory.StartNew(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _optionRepositoriesFactory
                        .NewOptionUnitRepository(connection)
                        .GetOptionUnit(optionUnitId);
                }
            });

        public Task<List<OptionUnit>> GetOptionUnitsByGroupIdAsync(long optionGroupId) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _optionRepositoriesFactory
                        .NewOptionUnitRepository(connection)
                        .GetOptionUnitsByGroup(optionGroupId);
                }
            });

        public Task<OptionUnit> AddOptionUnit(OptionUnit optionUnit) =>
            Task.Factory.StartNew(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    optionUnit.Id = _optionRepositoriesFactory
                        .NewOptionUnitRepository(connection)
                        .AddOptionUnit(optionUnit);

                    return optionUnit;
                }
            });

        public Task UpdateOptionUnit(OptionUnit optionUnit) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    _optionRepositoriesFactory
                        .NewOptionUnitRepository(connection)
                        .UpdateOptionUnit(optionUnit);
                }
            });

        public Task DeleteOptionUnit(long optionUnitId) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IOptionUnitRepository repository = _optionRepositoriesFactory.NewOptionUnitRepository(connection);

                    OptionUnit optionUnit = repository.GetOptionUnit(optionUnitId);

                    repository.UpdateOptionUnit(optionUnit);
                }
            });
    }
}
