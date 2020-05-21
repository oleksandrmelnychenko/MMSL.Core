using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
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
                    IMeasurementSizeRepository repository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);

                    return repository.GetMeasurementSizes(measurementId);
                }
            });
    }
}
