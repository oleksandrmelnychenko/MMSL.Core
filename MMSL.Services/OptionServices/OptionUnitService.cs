﻿using MMSL.Domain.DataContracts;
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

        public Task<OptionUnit> UpdateOptionUnit(OptionUnit optionUnit) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    _optionRepositoriesFactory
                        .NewOptionUnitRepository(connection)
                        .UpdateOptionUnit(optionUnit);

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