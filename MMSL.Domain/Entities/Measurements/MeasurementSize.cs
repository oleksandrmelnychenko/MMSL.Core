using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementSize : EntityBaseNamed {

        public MeasurementSize() {
            MeasurementMapSizes = new HashSet<MeasurementMapSize>();
            MeasurementMapValues = new HashSet<MeasurementMapValue>();
        }

        public ICollection<MeasurementMapSize> MeasurementMapSizes { get; set; }
        public ICollection<MeasurementMapValue> MeasurementMapValues { get; set; }
    }
}
