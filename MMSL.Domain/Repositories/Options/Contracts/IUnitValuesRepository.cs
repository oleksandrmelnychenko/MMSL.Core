using MMSL.Domain.Entities.Options;

namespace MMSL.Domain.Repositories.Options.Contracts {
    public interface IUnitValuesRepository {
        UnitValue GetUnitValuesByUnitId(long optionUnitId);
        UnitValue GetUnitValue(long unitValueId);
        long AddUnitValue(UnitValue value);
        UnitValue UpdateUnitValue(UnitValue value);
    }
}
