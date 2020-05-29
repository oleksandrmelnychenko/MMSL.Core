using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Measurements {
    public class UpdateMeasuremetSizeDataContract : EntityDataContractBase<MeasurementSize> {

        public long MeasurementId { get; set; }
        public string Name { get; set; }

        public List<UpdateValueDataContract> ValueDataContracts { get; set; }

        public override MeasurementSize GetEntity() {
            return new MeasurementSize {
                Id = Id,
                Name = Name
            };
        }
    }

    public class UpdateValueDataContract {
        public long Id { get; set; }
        public long MeasurementDefinitionId { get; set; }
        public float? Value { get; set; }
    }
}
