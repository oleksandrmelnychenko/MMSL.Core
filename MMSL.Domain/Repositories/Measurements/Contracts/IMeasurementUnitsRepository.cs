using MMSL.Domain.Entities.Measurements;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Measurements.Contracts {
    public interface IMeasurementUnitsRepository {
        List<MeasurementUnit> GetAll();
    }
}
