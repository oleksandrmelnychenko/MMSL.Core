using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System;
using System.Collections.Generic;
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
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);
                    IMeasurementDefinitionRepository definitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);
                    IMeasurementValueRepository valueRepository = _measurementsRepositoriesFactory.NewMeasurementValueRepository(connection);

                    measurementSize.Id = sizeRepository.AddMeasurementSize(measurementSize);

                    foreach (MeasurementValue measurementValue in measurementSize.Values) {
                        if (measurementValue.MeasurementDefinition.IsNew()) {
                            //TODO: create new definition and map
                            //AND save value 
                            
                        } else {
                            //TODO: save value

                        }
                    }

                    return measurementSize;
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
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);
                    IMeasurementDefinitionRepository definitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);
                    IMeasurementValueRepository valueRepository = _measurementsRepositoriesFactory.NewMeasurementValueRepository(connection);

                    measurementSize = sizeRepository.UpdateMeasurementSize(measurementSize);

                    foreach (MeasurementValue measurementValue in measurementSize.Values) {
                        if (measurementValue.MeasurementDefinition.IsNew()) {
                            //TODO: create new definition and map
                            //AND save value 

                        } else {
                            //TODO: save value

                        }
                    }

                    return new MeasurementSize();
                }
            });
    }
}
