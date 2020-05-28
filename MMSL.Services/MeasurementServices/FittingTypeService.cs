using MMSL.Domain.DataContracts.FittingTypes;
using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.MeasurementServices {
    public class FittingTypeService : IFittingTypeService {

        private readonly IMeasurementsRepositoriesFactory _measurementsRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public FittingTypeService(IMeasurementsRepositoriesFactory measurementsRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
            _measurementsRepositoriesFactory = measurementsRepositoriesFactory;
        }

        public Task<List<FittingType>> GetFittingTypesAsync(string searchPhrase) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var fittingTypeRepository = _measurementsRepositoriesFactory.NewFittingTypeRepository(connection);

                     return fittingTypeRepository.GetAll(searchPhrase); ;
                 }
             });

        public Task<FittingType> AddFittingTypeAsync(FittingTypeDataContract fittingTypeDataContract) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var fittingTypeRepository = _measurementsRepositoriesFactory.NewFittingTypeRepository(connection);
                     IMeasurementMapSizeRepository measurementMapSizeRepository = _measurementsRepositoriesFactory.NewMeasurementMapSizeRepository(connection);
                     IMeasurementMapValueRepository measurementMapValueRepository = _measurementsRepositoriesFactory.NewMeasurementMapValueRepository(connection);

                     FittingType newFittingType =
                        fittingTypeRepository.Add(fittingTypeDataContract.Type, fittingTypeDataContract.Unit, fittingTypeDataContract.DealerAccountId);

                     if (newFittingType != null) {

                         if (fittingTypeDataContract.ValueDataContracts != null && fittingTypeDataContract.ValueDataContracts.Any()) {
                             foreach (ValueDataContract valueDataContract in fittingTypeDataContract.ValueDataContracts) {
                                 measurementMapValueRepository.AddValue(new MeasurementMapValue {
                                     FittingTypeId = newFittingType.Id,
                                     MeasurementDefinitionId = valueDataContract.MeasurementDefinitionId,
                                     Value = valueDataContract.Value
                                 });
                             }
                         }
                     }
                     return newFittingType;
                 }
             });
    }
}
