using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
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
                    List<MeasurementDefinition> measurementDefinitions = null;
                    var measurementDefinitionRepository = _measurementsRepositoriesFactory.NewMeasurementDefinitionRepository(connection);
                    measurementDefinitions = measurementDefinitionRepository.GetAll(searchPhrase, isDefault);
                    return measurementDefinitions;
                }
            });

    }
}
