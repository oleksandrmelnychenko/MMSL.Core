using MMSL.Domain.Entities.Measurements;

namespace MMSL.Domain.DataContracts.Measurements {
    public class UpdateMeasurementMapDefinitionDataContract : EntityDataContractBase<MeasurementMapDefinition> {
        
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public long MapId { get; set; }

        public override MeasurementMapDefinition GetEntity() {
            return new MeasurementMapDefinition {
                Id = MapId,
                MeasurementDefinitionId = Id,
                IsDeleted = IsDeleted,
                MeasurementDefinition = new MeasurementDefinition { 
                    Id = Id,
                    Name = Name
                }
            };
        }
    }
}
