using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices {
    public class MeasurementService : IMeasurementService {
        private readonly IMeasurementsRepositoriesFactory _measurementsRepositoriesFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="measurementsRepositoriesFactory"></param>
        public MeasurementService(IMeasurementsRepositoriesFactory measurementsRepositoriesFactory) {
            _measurementsRepositoriesFactory = measurementsRepositoriesFactory;
        }

        public Task<List<Measurement>> GetMeasurementsAsync(string searchPhrase) {
            throw new System.NotImplementedException();
        }
    }
}
