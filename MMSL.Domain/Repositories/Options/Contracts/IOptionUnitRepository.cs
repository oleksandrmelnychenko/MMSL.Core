using MMSL.Domain.Entities.Options;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Options.Contracts {
    public interface IOptionUnitRepository {
        List<OptionUnit> GetOptionUnitsByGroup(long optionGroupId);
        OptionUnit GetOptionUnit(long optionUnitId);
        long AddOptionUnit(OptionUnit optionUnit);
        void UpdateOptionUnit(OptionUnit optionUnit);
        void UpdateOptionUnitIndex(long optionUnitId, int optionIndex);
    }
}
