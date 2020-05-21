using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.DataContracts.Measurements {
    public class MeasurementValueDataContract : EntityDataContractBase<MeasurementValue> {
        public float Value { get; set; }

        public long MeasurementDefinitionId { get; set; }

        public string MeasurementDefinitionName { get; set; }

        public override MeasurementValue GetEntity() {
            return new MeasurementValue {
                Value = Value,
                Id = Id,
                MeasurementDefinitionId = MeasurementDefinitionId,
                MeasurementDefinition = new MeasurementDefinition { 
                    Name = MeasurementDefinitionName,
                    Id = MeasurementDefinitionId
                }
            };
        }
    }
}
