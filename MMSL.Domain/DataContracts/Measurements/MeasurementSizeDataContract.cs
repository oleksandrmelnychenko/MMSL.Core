using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;
using System.Linq;

namespace MMSL.Domain.DataContracts.Measurements {
    public class MeasurementSizeDataContract : EntityDataContractBase<MeasurementSize> {

        public string Name { get; set; }

        public string Description { get; set; }

        public long MeasurementId { get; set; }

        public List<MeasurementValueDataContract> Values { get; set; }

        //TODO: update this
        public override MeasurementSize GetEntity() {
            return new MeasurementSize {
                Id = Id,
                Name = Name,
                Description = Description,
                //MeasurementId = MeasurementId,
                //Values = Values.Select(x => new MeasurementValue {
                //    Id = x.Id,
                //    Value = x.Value,
                //    MeasurementDefinitionId = x.MeasurementDefinitionId,
                //    MeasurementDefinition = new MeasurementDefinition {
                //        Id = x.MeasurementDefinitionId,
                //        Name = x.MeasurementDefinitionName
                //    }
                //}).ToList()
            };
        }
    }
}
