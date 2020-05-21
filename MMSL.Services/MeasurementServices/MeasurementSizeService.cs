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
                        .GetMeasurementSizes(measurementId);
                }
            });

        public Task<MeasurementSize> AddMeasurementSize(MeasurementSize measurementSize) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);

                    CheckMeasurementValues(connection, measurementSize.MeasurementId, measurementSize.Values);

                    measurementSize.Id = sizeRepository.AddMeasurementSize(measurementSize);

                    return sizeRepository.GetMeasurementSize(measurementSize.Id);
                }
            });

        public Task<MeasurementSize> DeleteMeasurementSize(MeasurementSize measurementSize) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    //TODO: 
                    throw new NotImplementedException();

                    return new MeasurementSize();
                }
            });

        public Task<MeasurementSize> UpdateMeasurementSize(MeasurementSize measurementSize) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);

                    CheckMeasurementValues(connection, measurementSize.MeasurementId, measurementSize.Values);

                    sizeRepository.UpdateMeasurementSize(measurementSize);

                    return sizeRepository.GetMeasurementSize(measurementSize.Id);
                }
            });

        private void CheckMeasurementValues(IDbConnection connection, long measurementId, IEnumerable<MeasurementValue> values) {
            IMeasurementDefinitionRepository definitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);
            IMeasurementValueRepository valueRepository = _measurementsRepositoriesFactory.NewMeasurementValueRepository(connection);
            IMeasurementMapDefinitionRepository mapRepository = _measurementsRepositoriesFactory.NewMeasurementMapDefinitionRepository(connection);


            foreach (MeasurementValue measurementValue in values) {
                if (measurementValue.MeasurementDefinition.IsNew()) {

                    MeasurementDefinition definition = definitionRepository.NewMeasurementDefinition(measurementValue.MeasurementDefinition);

                    MeasurementMapDefinition map = new MeasurementMapDefinition {
                        MeasurementDefinitionId = definition.Id,
                        MeasurementId = measurementId
                    };

                    long mapId = mapRepository.AddMeasurementMapDefinition(map);
                }

                if (measurementValue.IsNew()) {
                    measurementValue.Id = valueRepository.AddValue(measurementValue);
                } else {
                    valueRepository.UpdateValue(measurementValue);
                }
            }
        }
    }
}
