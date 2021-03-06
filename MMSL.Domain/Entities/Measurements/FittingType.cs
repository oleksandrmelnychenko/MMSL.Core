﻿using MMSL.Domain.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Entities.Measurements {
    public class FittingType : EntityBase {

        public FittingType() {
            MeasurementMapValues = new HashSet<MeasurementMapValue>();
        }

        public string Type { get; set; }

        public long MeasurementId { get; set; }
        public Measurement Measurement { get; set; }

        public ICollection<MeasurementMapValue> MeasurementMapValues { get; set; }
    }
}
