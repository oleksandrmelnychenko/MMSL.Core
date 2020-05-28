using System.Collections.Generic;

namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementMapValue : EntityBase {
        public long? FittingTypeId { get; set; }
        public FittingType FittingType { get; set; }

        public long? MeasurementId { get; set; }
        public Measurement Measurement { get; set; }

        public long? MeasurementSizeId { get; set; }
        public MeasurementSize MeasurementSize { get; set; }

        public long MeasurementDefinitionId { get; set; }
        public MeasurementDefinition MeasurementDefinition { get; set; }

        public float Value { get; set; }
    }
}
