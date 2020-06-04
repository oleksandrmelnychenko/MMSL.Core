using MMSL.Domain.DataContracts.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts.Measurements {
    public class NewMeasurementDataContract {
        public long? ProductCategoryId { get; set; }

        public long? BaseMeasurementId { get; set; }

        public string Name { get; set; }

        public List<MeasurementDefinitionDataContract> MeasurementDefinitions { get; set; } = new List<MeasurementDefinitionDataContract>();
    }
}
