using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices {
    public class MeasurementDefinitionService : IMeasurementDefinitionService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IMeasurementsRepositoriesFactory _measurementsRepositoriesFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="measurementsRepositoriesFactory"></param>
        /// <param name="connectionFactory"></param>
        public MeasurementDefinitionService(IMeasurementsRepositoriesFactory measurementsRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _measurementsRepositoriesFactory = measurementsRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<List<MeasurementDefinition>> GetMeasurementDefinitionsAsync(string searchPhrase, bool? isDefault) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    var measurementDefinitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);

                    return measurementDefinitionRepository.GetAll(searchPhrase, isDefault); ;
                }
            });

        public Task<MeasurementDefinition> NewMeasurementDefinitionAsync(NewMeasurementDefinitionDataContract newMeasurementDefinitionDataContract) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    var measurementDefinitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);

                    MeasurementDefinition measurementDefinition = measurementDefinitionRepository
                        .NewMeasurementDefinition(new MeasurementDefinition {
                            Name = newMeasurementDefinitionDataContract.Name,
                            Description = newMeasurementDefinitionDataContract.Description,
                            IsDefault = newMeasurementDefinitionDataContract.IsDefault
                        });

                    return measurementDefinition;
                }
            });

        public Task UpdateMeasurementDefinitionAsync(MeasurementDefinition measurementDefinition) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    var measurementDefinitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);

                    measurementDefinitionRepository.UpdateMeasurementDefinition(measurementDefinition);
                }
            });

        public Task DeleteMeasurementDefinitionAsync(long measurementDefinitionId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    var measurementDefinitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);

                    MeasurementDefinition measurementDefinition = measurementDefinitionRepository.GetById(measurementDefinitionId);

                    if (measurementDefinition == null) throw new Exception("MeasurementDefinition not found");

                    measurementDefinition.IsDeleted = true;

                    measurementDefinitionRepository.UpdateMeasurementDefinition(measurementDefinition);
                }
            });

    }
}
