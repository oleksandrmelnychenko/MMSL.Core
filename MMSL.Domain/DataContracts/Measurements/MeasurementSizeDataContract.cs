using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Linq;

namespace MMSL.Domain.DataContracts.Measurements {
    public class MeasurementSizeDataContract : EntityDataContractBase<MeasurementSize> {

        public string Name { get; set; }

        public string Description { get; set; }

        public List<MeasurementValueDataContract> Values { get; set; }

        public override MeasurementSize GetEntity() {
            return new MeasurementSize {
                Name = Name,
                Description = Description,
                Values = Values.Select(x => new MeasurementValue {
                    Id = x.Id,
                    Value = x.Value,
                    MeasurementDefinitionId = x.MeasurementDefinitionId,
                    MeasurementDefinition = new MeasurementDefinition {
                        Id = x.MeasurementDefinitionId,
                        Name = x.MeasurementDefinitionName
                    }
                }).ToList()
            };
        }
    }
}
