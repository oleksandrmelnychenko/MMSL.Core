using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;

namespace MMSL.Services.MeasurementServices {
    public class MeasurementDefinitionService : IMeasurementDefinitionService {
        private readonly IMeasurementsRepositoriesFactory _measurementsRepositoriesFactory;

        public MeasurementDefinitionService(IMeasurementsRepositoriesFactory measurementsRepositoriesFactory) {
            _measurementsRepositoriesFactory = measurementsRepositoriesFactory;
        }
    }
}
