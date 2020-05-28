using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Measurements {
    public class UpdateMeasurementMapSizeDataContract : EntityDataContractBase<MeasurementMapSize> {

        public long MeasurementSizeId { get; set; }

        public bool IsDeleted { get; set; }

        public string SizeName { get; set; }

        public List<MeasurementValueDataContract> Values { get; set; } = new List<MeasurementValueDataContract>();

        public override MeasurementMapSize GetEntity() {
            return new MeasurementMapSize {
                Id = Id,
                MeasurementSizeId = MeasurementSizeId,
                IsDeleted = IsDeleted,
                MeasurementSize = new MeasurementSize {
                    Id = MeasurementSizeId,
                    Name = SizeName
                }
            };
        }
    }
}
