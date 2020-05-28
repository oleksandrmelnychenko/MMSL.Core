using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Measurements {
    public class UpdateMeasuremetSizeDataContract : EntityDataContractBase<MeasurementMapSize> {

        public long SizeMapId { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public List<UpdateValueDataContract> ValueDataContracts { get; set; }

        public override MeasurementMapSize GetEntity() {
            return new MeasurementMapSize {
                Id = SizeMapId,
                IsDeleted = IsDeleted,
                MeasurementSizeId = Id,
                MeasurementSize = new MeasurementSize {
                    Id = Id,
                    Name = Name
                }
            };
        }
    }

    public class UpdateValueDataContract {
        public long Id { get; set; }
        public float? Value { get; set; }
    }
}
