using System.Collections.Generic;

namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementDefinition : EntityBaseNamed {

        public MeasurementDefinition() {
            MeasurementMapDefinitions = new HashSet<MeasurementMapDefinition>();
            MeasurementMapValues = new HashSet<MeasurementMapValue>();
        }

        public bool IsDefault { get; set; }

        public ICollection<MeasurementMapDefinition> MeasurementMapDefinitions { get; set; }
        public ICollection<MeasurementMapValue> MeasurementMapValues { get; set; }
    }
}
