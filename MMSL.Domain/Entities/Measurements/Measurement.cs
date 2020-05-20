using MMSL.Domain.Entities.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Measurements {
    public class Measurement : EntityBaseNamed {

        public Measurement() {
            MeasurementMapDefinitions = new HashSet<MeasurementMapDefinition>();
            MeasurementSizes = new HashSet<MeasurementSize>();
        }

        public long ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public ICollection<MeasurementMapDefinition> MeasurementMapDefinitions { get; set; }
        public ICollection<MeasurementSize> MeasurementSizes { get; set; }
    }
}
