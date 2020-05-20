using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Measurements {
    public class Measurement : EntityBaseNamed {
        public ICollection<MeasurementMapDefinition> MeasurementMapDefinitions { get; set; }
        public ICollection<MeasurementSize> MeasurementSizes { get; set; }


    }
}
