using MMSL.Common.Exceptions.UserExceptions;
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

        public Task<List<FittingType>> GetFittingTypesAsync(string searchPhrase, long measurementId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var fittingTypeRepository = _measurementsRepositoriesFactory.NewFittingTypeRepository(connection);

                     return fittingTypeRepository.GetAll(searchPhrase, measurementId); ;
                 }
             });

        public Task<FittingType> GetFittingTypeByIdAsync(long fittingTypeId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    var fittingTypeRepository = _measurementsRepositoriesFactory.NewFittingTypeRepository(connection);
                    FittingType fittingType = fittingTypeRepository.GetById(fittingTypeId);
                    return fittingType;
                }
            });

        public Task<FittingType> AddFittingTypeAsync(FittingTypeDataContract fittingTypeDataContract) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var fittingTypeRepository = _measurementsRepositoriesFactory.NewFittingTypeRepository(connection);
                     IMeasurementMapValueRepository measurementMapValueRepository = _measurementsRepositoriesFactory.NewMeasurementMapValueRepository(connection);

                     FittingType newFittingType =
                        fittingTypeRepository.Add(fittingTypeDataContract.Type, fittingTypeDataContract.MeasurementUnitId, fittingTypeDataContract.MeasurementId);

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

        public Task<FittingType> UpdateFittingTypeAsync(FittingType fittingType) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     var fittingTypeRepository = _measurementsRepositoriesFactory.NewFittingTypeRepository(connection);
                     IMeasurementMapValueRepository measurementMapValueRepository = _measurementsRepositoriesFactory.NewMeasurementMapValueRepository(connection);

                     fittingTypeRepository.Update(fittingType);

                     if (fittingType.MeasurementMapValues.Any()) {
                         foreach (MeasurementMapValue value in fittingType.MeasurementMapValues) {
                             if (value.IsNew()) {
                                 measurementMapValueRepository.AddValue(value);
                             } else {
                                 measurementMapValueRepository.UpdateValue(value);
                             }
                         }
                     }

                     return fittingTypeRepository.GetById(fittingType.Id);
                 }
             });

        public Task DeleteFittingTypeAsync(long fittingTypeId) =>
             Task.Run(() => {
                 using (var connection = _connectionFactory.NewSqlConnection()) {
                     var fittingTypeRepository = _measurementsRepositoriesFactory.NewFittingTypeRepository(connection);

                     FittingType existed = fittingTypeRepository.GetById(fittingTypeId);

                     if (existed != null) {
                         existed.IsDeleted = true;
                         fittingTypeRepository.Update(existed);
                     } else {
                         UserExceptionCreator<NotFoundValueException>.Create(NotFoundValueException.VALUE_NOT_FOUND).Throw();
                     }
                 }
             });
    }
}
