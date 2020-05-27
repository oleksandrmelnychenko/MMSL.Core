namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementValue : EntityBase {

        public float Value { get; set; }

        public long MeasurementMapValueId { get; set; }
        public MeasurementMapValue MeasurementMapValue { get; set; }


        //public long MeasurementDefinitionId { get; set; }

        //public MeasurementDefinition MeasurementDefinition { get; set; }

        //public long? MeasurementSizeId { get; set; }

        //public MeasurementSize MeasurementSize { get; set; }
    }
}
