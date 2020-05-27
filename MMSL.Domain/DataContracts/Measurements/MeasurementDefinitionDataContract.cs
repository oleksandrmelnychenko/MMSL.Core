using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.DataContracts.Measurements {
    public class MeasurementDefinitionDataContract : EntityDataContractBase<MeasurementDefinition> {

        public string Name { get; set; }

        public override MeasurementDefinition GetEntity() {
            return new MeasurementDefinition {
                Id = Id,
                Name = Name
            };
        }
    }
}
