using MMSL.Domain.DataContracts;
using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices {
    public class MeasurementService : IMeasurementService {

        private readonly IMeasurementsRepositoriesFactory _measurementsRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="measurementsRepositoriesFactory"></param>
        public MeasurementService(IMeasurementsRepositoriesFactory measurementsRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _measurementsRepositoriesFactory = measurementsRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<List<Measurement>> GetMeasurementsAsync(string searchPhrase) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     return _measurementsRepositoriesFactory
                         .NewMeasurementRepository(connection)
                         .GetAll(searchPhrase);
                 }
             });

        public Task<Measurement> GetMeasurementDetailsAsync(long measurementId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     return _measurementsRepositoriesFactory
                        .NewMeasurementRepository(connection)
                        .GetByIdWithDefinitions(measurementId);
                 }
             });

        public Task<Measurement> NewMeasurementAsync(NewMeasurementDataContract newMeasurementDataContract) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     IMeasurementRepository measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);

                     IMeasurementDefinitionRepository definitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);
                     IMeasurementMapDefinitionRepository mapDefinitionRepository = _measurementsRepositoriesFactory.NewMeasurementMapDefinitionRepository(connection);

                     Measurement measurement = measurementRepository.NewMeasurement(newMeasurementDataContract);

                     foreach (MeasurementDefinitionDataContract definition in newMeasurementDataContract.MeasurementDefinitions) {
                         MeasurementDefinition definitionEntity = definition.GetEntity();

                         if (definitionEntity.IsNew()) {
                             definitionEntity = definitionRepository.NewMeasurementDefinition(definitionEntity);
                         }

                         MeasurementMapDefinition definitionMap = new MeasurementMapDefinition() {
                             MeasurementDefinitionId = definitionEntity.Id,
                             MeasurementId = measurement.Id,
                             OrderIndex = definition.OrderIndex
                         };

                         definitionMap.Id = mapDefinitionRepository.AddMeasurementMapDefinition(definitionMap);
                     }

                     return measurement;
                 }
             });

        public Task<Measurement> UpdateMeasurementAsync(UpdateMeasurementDataContract measurement) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementRepository measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);

                    Measurement measurementEntity = measurement.GetEntity();
                    measurementRepository.UpdateMeasurement(measurementEntity);

                    if (measurement.MeasurementDefinitions.Any()) {
                        IMeasurementDefinitionRepository definitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);
                        IMeasurementMapDefinitionRepository mapDefinitionRepository = _measurementsRepositoriesFactory.NewMeasurementMapDefinitionRepository(connection);

                        foreach (var definitionDataContract in measurement.MeasurementDefinitions) {
                            MeasurementMapDefinition definitionMap = definitionDataContract.GetEntity();
                            definitionMap.MeasurementId = measurement.Id;

                            MeasurementDefinition definition = definitionMap.MeasurementDefinition;

                            if (definitionMap.IsNew()) {
                                // TODO: check this 'if(definition.IsNew())' if definition selection possible 
                                // (ex. for child measurement carts)
                                definition = definitionRepository.NewMeasurementDefinition(definition);
                                definitionMap.MeasurementDefinition = definition;
                                definitionMap.MeasurementDefinitionId = definition.Id;

                                definitionMap.Id = mapDefinitionRepository.AddMeasurementMapDefinition(definitionMap);
                            } else if (definitionMap.IsDeleted) {

                                definitionMap = mapDefinitionRepository.UpdateMeasurementMapDefinition(definitionMap);
                            } else {

                                Measurement mapOwner = measurementRepository.GetById(measurement.Id);
                                MeasurementDefinition originalDefinition = definitionRepository.GetById(definition.Id);

                                if (originalDefinition.Name != definition.Name) {
                                    if (mapOwner.ParentMeasurementId.HasValue) {

                                        definition = definitionRepository.NewMeasurementDefinition(
                                            new MeasurementDefinition {
                                                Name = definition.Name,
                                                Description = definition.Description
                                            });

                                        definitionMap.MeasurementDefinitionId = definition.Id;

                                    } else {
                                        definitionRepository.UpdateMeasurementDefinition(definition);
                                    }
                                }

                                definitionMap = mapDefinitionRepository.UpdateMeasurementMapDefinition(definitionMap);
                            }

                            measurementEntity.MeasurementMapDefinitions.Add(definitionMap);
                        }
                    }

                    return measurementEntity;
                }
            });

        public Task DeleteMeasurementAsync(long measurementId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);

                     Measurement measurement = measurementRepository.GetById(measurementId);

                     if (measurement == null) throw new Exception("Measurement not found");

                     measurement.IsDeleted = true;

                     measurementRepository.UpdateMeasurement(measurement);
                 }
             });

        public Task<List<Measurement>> GetProductMeasurementsAsync(long productCategoryId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     return _measurementsRepositoriesFactory
                        .NewMeasurementRepository(connection)
                        .GetAllByProduct(productCategoryId);
                 }
             });

        public Task<Measurement> GetMeasurementChartAsync(long measurementId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var repository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);

                     Measurement measurement = repository.GetByIdWithDefinitions(measurementId);
                     measurement.MeasurementMapSizes = repository.GetSizesByMeasurementId(measurement.Id, measurement.ParentMeasurementId);

                     return measurement;
                 }
             });
    }
}
