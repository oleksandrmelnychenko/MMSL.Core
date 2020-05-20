using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Measurements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementRepository {
        List<Measurement> GetAll(string searchPhrase);
        Measurement NewMeasurement(NewMeasurementDataContract newMeasurementDataContract);
    }
}
