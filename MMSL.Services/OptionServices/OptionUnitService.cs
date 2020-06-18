using Microsoft.EntityFrameworkCore.Internal;
using MMSL.Domain.DataContracts.ProductOptions;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using MMSL.Services.OptionServices.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public Task<OptionUnit> AddOptionUnit(OptionUnit optionUnit, List<UnitValueDataContract> values) =>
            Task.Factory.StartNew(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IOptionGroupRepository groupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);
                    IOptionUnitRepository unitRepository = _optionRepositoriesFactory.NewOptionUnitRepository(connection);
                    IUnitValuesRepository unitValuesRepository = _optionRepositoriesFactory.NewUnitValuesRepository(connection);

                    List<OptionUnit> units = unitRepository.GetOptionUnitsByGroup(optionUnit.OptionGroupId);
                    if (units.Any()) {
                        optionUnit.OrderIndex = units.Count;
                    }

                    optionUnit.Id = unitRepository.AddOptionUnit(optionUnit);

                    if (values != null) {
                        foreach (UnitValueDataContract value in values) {
                            unitValuesRepository.AddUnitValue(new UnitValue {
                                Value = value.Value,
                                OptionUnitId = optionUnit.Id
                            });
                        }
                    }

                    return optionUnit;
                }
            });

        public Task<OptionUnit> UpdateOptionUnit(OptionUnit optionUnit, List<UnitValueDataContract> values) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IUnitValuesRepository unitValuesRepository = _optionRepositoriesFactory.NewUnitValuesRepository(connection);

                    _optionRepositoriesFactory
                        .NewOptionUnitRepository(connection)
                        .UpdateOptionUnit(optionUnit);

                    if (values != null) {
                        foreach (UnitValueDataContract value in values) {
                            var entity = new UnitValue {
                                Id = value.Id,
                                Value = value.Value,
                                OptionUnitId = optionUnit.Id
                            };

                            if (entity.IsNew()) {
                                unitValuesRepository.AddUnitValue(entity);
                            } else {
                                unitValuesRepository.UpdateUnitValue(entity);
                            }
                        }
                    }

                    return optionUnit;
                }
            });

        public Task<OptionUnit> DeleteOptionUnit(long optionUnitId) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IOptionUnitRepository repository = _optionRepositoriesFactory.NewOptionUnitRepository(connection);

                    OptionUnit optionUnit = repository.GetOptionUnit(optionUnitId);

                    optionUnit.IsDeleted = true;

                    repository.UpdateOptionUnit(optionUnit);

                    return optionUnit;
                }
            });

        public Task UpdateOrderIndexesAsync(List<UpdateOrderIndexDataContract> orderIndexes) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IOptionUnitRepository repository = _optionRepositoriesFactory.NewOptionUnitRepository(connection);

                    foreach (var item in orderIndexes) {
                        OptionUnit exist = repository.GetOptionUnit(item.OptionUnitId);
                        if (exist != null) {
                            repository.UpdateOptionUnitIndex(item.OptionUnitId, item.OrderIndex);
                        }
                    }
                }
            });
    }
}
