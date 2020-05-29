using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Services.MeasurementServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public Task<List<MeasurementSize>> GetMeasurementSizesAsync(long measurementId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _measurementsRepositoriesFactory
                        .NewMeasurementSizeRepository(connection)
                        .GetAllByMeasurementId(measurementId);
                }
            });

        public Task<MeasurementSize> AddMeasurementSizeAsync(MeasurementSizeDataContract measurementSizeDataContract) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);
                    IMeasurementMapSizeRepository measurementMapSizeRepository = _measurementsRepositoriesFactory.NewMeasurementMapSizeRepository(connection);
                    IMeasurementMapValueRepository measurementMapValueRepository = _measurementsRepositoriesFactory.NewMeasurementMapValueRepository(connection);

                    MeasurementSize newMeasurementSize = sizeRepository.AddMeasurementSize(measurementSizeDataContract.Name, measurementSizeDataContract.Description);

                    if (newMeasurementSize != null) {
                        measurementMapSizeRepository.New(measurementSizeDataContract.MeasurementId, newMeasurementSize.Id);

                        if (measurementSizeDataContract.ValueDataContracts != null && measurementSizeDataContract.ValueDataContracts.Any()) {
                            foreach (ValueDataContract valueDataContract in measurementSizeDataContract.ValueDataContracts) {
                                measurementMapValueRepository.AddValue(new MeasurementMapValue {
                                    MeasurementId = measurementSizeDataContract.MeasurementId,
                                    MeasurementSizeId = newMeasurementSize.Id,
                                    MeasurementDefinitionId = valueDataContract.MeasurementDefinitionId,
                                    Value = valueDataContract.Value
                                });
                            }
                        }
                    }
                    return newMeasurementSize;
                }
            });

        public Task<MeasurementSize> DeleteMeasurementSizeAsync(long measurementId, long measurementSizeId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementRepository measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);

                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);
                    IMeasurementMapSizeRepository sizeMapRepository = _measurementsRepositoriesFactory.NewMeasurementMapSizeRepository(connection);

                    Measurement measurement = measurementRepository.GetById(measurementId);

                    MeasurementSize size = sizeRepository.GetById(measurementSizeId);

                    if (!measurement.ParentMeasurementId.HasValue) {
                        size.IsDeleted = true;
                        sizeRepository.UpdateMeasurementSize(size);
                    }

                    MeasurementMapSize sizeMap = sizeMapRepository.Get(measurementId, measurementSizeId);

                    sizeMap.IsDeleted = true;
                    sizeMapRepository.Update(sizeMap);

                    return size;
                }
            });

        public Task<MeasurementSize> UpdateMeasurementSizeAsync(UpdateMeasuremetSizeDataContract measurementSize) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IMeasurementRepository measurementRepository = _measurementsRepositoriesFactory.NewMeasurementRepository(connection);
                    IMeasurementSizeRepository sizeRepository = _measurementsRepositoriesFactory.NewMeasurementSizeRepository(connection);
                    IMeasurementMapSizeRepository sizeMapRepository = _measurementsRepositoriesFactory.NewMeasurementMapSizeRepository(connection);
                    IMeasurementMapValueRepository sizeValueRepository = _measurementsRepositoriesFactory.NewMeasurementMapValueRepository(connection);

                    Measurement measurement = measurementRepository.GetById(measurementSize.MeasurementId);
                    MeasurementSize originalSize = sizeRepository.GetById(measurementSize.Id);

                    if (!measurement.ParentMeasurementId.HasValue) {
                        originalSize.Name = measurementSize.Name;
                        sizeRepository.UpdateMeasurementSize(originalSize);
                    } else {
                        //MeasurementMapSize sizeMap = sizeMapRepository.Get(measurement.Id, originalSize.Id);

                        //  TODO: update OR create size
                        //  AND resolve parentel measurement charts

                        //  TODO: investigate this
                    }

                    // update values
                    foreach (UpdateValueDataContract valueModel in measurementSize.ValueDataContracts) {
                        MeasurementMapValue valEntity = sizeValueRepository.GetValue(valueModel.Id);

                        if (valEntity == null || valEntity.MeasurementId != measurement.Id) {
                            sizeValueRepository.AddValue(new MeasurementMapValue {
                                MeasurementId = measurement.Id,
                                MeasurementSizeId = originalSize.Id,
                                MeasurementDefinitionId = valueModel.MeasurementDefinitionId,
                                Value = valueModel.Value
                            });
                        } else {
                            valEntity.Value = valueModel.Value;
                            sizeValueRepository.UpdateValue(valEntity);
                        }
                    }

                    return sizeRepository.GetById(measurementSize.Id);
                }
            });
    }
}
