namespace MMSL.Domain.Entities.Measurements {
    public class MeasurementMapSize : EntityBase {
        public long MeasurementId { get; set; }

        public Measurement Measurement { get; set; }

        public long MeasurementSizeId { get; set; }

        public MeasurementSize MeasurementSize { get; set; }
    }
}
