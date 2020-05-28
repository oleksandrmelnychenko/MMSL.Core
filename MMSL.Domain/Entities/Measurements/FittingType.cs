using MMSL.Domain.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Entities.Measurements {
    public class FittingType : EntityBase {

        public FittingType() {
            MeasurementMapValues = new HashSet<MeasurementMapValue>();
        }

        public string Type { get; set; }

        public string Unit { get; set; }

        public long DealerAccountId { get; set; }
        public DealerAccount DealerAccount { get; set; }

        public ICollection<MeasurementMapValue> MeasurementMapValues { get; set; }
    }
}
