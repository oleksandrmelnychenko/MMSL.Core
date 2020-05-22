using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
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
                     List<Measurement> measurements = null;
                     var measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);
                     measurements = measurementRepository.GetAll(searchPhrase);
                     return measurements;
                 }
             });

        public Task<Measurement> GetMeasurementDetailsAsync(long measurementId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     return _measurementsRepositoriesFactory
                        .NewMeasurementRepository(connection)
                        .GetByIdWithDependencies(measurementId);
                 }
             });

        public Task<Measurement> NewMeasurementAsync(NewMeasurementDataContract newMeasurementDataContract) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);

                     Measurement measurement = null;

                     measurement = measurementRepository.NewMeasurement(newMeasurementDataContract);

                     return measurement;
                 }
             });

        public Task UpdateMeasurementAsync(Measurement measurement) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    var measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);

                    measurementRepository.UpdateMeasurement(measurement);
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

    }
}
