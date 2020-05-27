using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices {    
    public class MeasurementSizeService : IMeasurementSizeService {
        private readonly IMeasurementsRepositoriesFactory _measurementsRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public MeasurementSizeService(IMeasurementsRepositoriesFactory measurementsRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _measurementsRepositoriesFactory = measurementsRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<List<MeasurementSize>> GetMeasurementSizes(long measurementId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _measurementsRepositoriesFactory
                        .NewMeasurementSizeRepository(connection)
                        .GetAllByMeasurementId(measurementId);
                }
            });

        public Task<MeasurementSize> AddMeasurementSize(MeasurementSizeDataContract measurementSizeDataContract) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);
                    IMeasurementMapSizeRepository measurementMapSizeRepository = _measurementsRepositoriesFactory.NewMeasurementMapSizeRepository(connection);

                    MeasurementSize newMeasurementSize = sizeRepository.AddMeasurementSize(measurementSizeDataContract.Name, measurementSizeDataContract.Description);

                    if (newMeasurementSize != null) {
                        measurementMapSizeRepository.New(measurementSizeDataContract.MeasurementId, newMeasurementSize.Id);
                    }

                    return newMeasurementSize;
                }
            });

        public Task<MeasurementSize> DeleteMeasurementSize(long measurementSizeId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);

                    MeasurementSize size = sizeRepository.GetById(measurementSizeId);

                    size.IsDeleted = true;

                    sizeRepository.UpdateMeasurementSize(size);

                    return size;
                }
            });

        public Task<MeasurementSize> UpdateMeasurementSize(MeasurementSize measurementSize) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);

                    sizeRepository.UpdateMeasurementSize(measurementSize);

                    //CheckMeasurementValues(connection, measurementSize.Id, measurementSize.MeasurementId, measurementSize.Values);

                    return sizeRepository.GetById(measurementSize.Id);
                }
            });

        //TODO: update this
        private void CheckMeasurementValues(IDbConnection connection, long sizeId, long measurementId, IEnumerable<MeasurementMapValue> values) {
            IMeasurementDefinitionRepository definitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);
            IMeasurementValueRepository valueRepository = _measurementsRepositoriesFactory.NewMeasurementValueRepository(connection);
            IMeasurementMapDefinitionRepository mapRepository = _measurementsRepositoriesFactory.NewMeasurementMapDefinitionRepository(connection);


            foreach (MeasurementMapValue measurementValue in values) {
                if (measurementValue.MeasurementDefinition.IsNew()) {

                    MeasurementDefinition definition = definitionRepository.NewMeasurementDefinition(measurementValue.MeasurementDefinition);

                    MeasurementMapDefinition map = new MeasurementMapDefinition {
                        MeasurementDefinitionId = definition.Id,
                        MeasurementId = measurementId
                    };

                    measurementValue.MeasurementDefinitionId = definition.Id;

                    map.Id = mapRepository.AddMeasurementMapDefinition(map);
                }

                measurementValue.MeasurementSizeId = sizeId;

                if (measurementValue.IsNew()) {
                    measurementValue.Id = valueRepository.AddValue(measurementValue);
                } else {
                    valueRepository.UpdateValue(measurementValue);
                }
            }
        }
    }
}
