using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;

namespace MMSL.Services.MeasurementServices {
    public class MeasurementService : IMeasurementService {
        private readonly IMeasurementsRepositoriesFactory _measurementsRepositoriesFactory;

        public MeasurementService(IMeasurementsRepositoriesFactory measurementsRepositoriesFactory) {
            _measurementsRepositoriesFactory = measurementsRepositoriesFactory;
        }
    }
}
