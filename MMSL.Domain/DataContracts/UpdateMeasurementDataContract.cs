using MMSL.Domain.DataContracts.Measurements;
using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.DataContracts {
    public class UpdateMeasurementDataContract : EntityDataContractBase<Measurement> {

        public string Name { get; set; }

        public List<UpdateMeasurementMapDefinitionDataContract> MeasurementDefinitions { get; set; } = new List<UpdateMeasurementMapDefinitionDataContract>();

        public override Measurement GetEntity() {
            return new Measurement {
                Id = Id,
                Name = Name
            };
        }
    }
}
