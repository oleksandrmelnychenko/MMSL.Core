using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts.DeliveryTimelines {
    public class NewDeliveryTimelineDataContract {
        public string Name { get; set; }

        public string Ivory { get; set; }

        public string Silver { get; set; }

        public string Black { get; set; }

        public string Gold { get; set; }

        public bool IsDefault { get; set; }
    }
}
