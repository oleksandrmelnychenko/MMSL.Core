using System.Collections.Generic;

namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementDefinition : EntityBaseNamed {
        public bool IsDefault { get; set; }

        public ICollection<MeasurementMapDefinition> MeasurementMapDefinitions { get; set; }
    }
}
