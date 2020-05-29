using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.DataContracts.Measurements {
    public class MeasurementValueDataContract : EntityDataContractBase<MeasurementMapValue> {
        public float Value { get; set; }

        public long MeasurementDefinitionId { get; set; }

        public string MeasurementDefinitionName { get; set; }

        public override MeasurementMapValue GetEntity() {
            return new MeasurementMapValue {
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
