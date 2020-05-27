using MMSL.Domain.Entities.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Measurements {
    public class Measurement : EntityBaseNamed {

        public Measurement() {
            MeasurementMapDefinitions = new HashSet<MeasurementMapDefinition>();
            MeasurementMapSizes = new HashSet<MeasurementMapSize>();
        }

        public long? ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public long? ParentMeasurementId { get; set; }
        public Measurement ParentMeasurement { get; set; }

        public ICollection<MeasurementMapSize> MeasurementMapSizes { get; set; }

        public ICollection<MeasurementMapDefinition> MeasurementMapDefinitions { get; set; }

        public ICollection<MeasurementMapValue> MeasurementMapValues { get; set; }
    }
}
