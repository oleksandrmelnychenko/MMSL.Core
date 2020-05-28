namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementMapDefinition : EntityBase {
        
        public int OrderIndex { get; set; }

        public long MeasurementId { get; set; }

        public Measurement Measurement { get; set; }

        public long MeasurementDefinitionId { get; set; }

        public MeasurementDefinition MeasurementDefinition { get; set; }
    }
}
