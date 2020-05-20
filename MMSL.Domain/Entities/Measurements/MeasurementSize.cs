using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementSize : EntityBaseNamed {

        public long MeasurementId { get; set; }

        public Measurement Measurement { get; set; }

        public ICollection<MeasurementValue> Values { get; set; }
    }
}
