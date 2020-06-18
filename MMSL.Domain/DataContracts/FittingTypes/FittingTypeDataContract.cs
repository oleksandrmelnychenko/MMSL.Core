using MMSL.Domain.DataContracts.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.FittingTypes {
    public  class FittingTypeDataContract {
        public string Type { get; set; }

        public long MeasurementUnitId { get; set; }

        public long MeasurementId { get; set; }

        public List<ValueDataContract> ValueDataContracts { get; set; }
    }
}
