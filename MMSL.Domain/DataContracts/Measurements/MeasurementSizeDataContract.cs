using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Measurements {
    public class MeasurementSizeDataContract  {     
        public string Name { get; set; }

        public string Description { get; set; }

        public long MeasurementId { get; set; }

        public List<ValueDataContract> ValueDataContracts { get; set; }
    }   
}
