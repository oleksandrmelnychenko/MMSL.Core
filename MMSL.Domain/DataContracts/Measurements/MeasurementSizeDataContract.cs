using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Linq;

namespace MMSL.Domain.DataContracts.Measurements {
    public class MeasurementSizeDataContract  {

        public string Name { get; set; }

        public string Description { get; set; }

        public long MeasurementId { get; set; }     
    }
}
