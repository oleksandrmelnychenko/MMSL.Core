using MMSL.Domain.Entities.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.Options.Contracts {
    public interface IOptionGroupRepository {

        List<OptionGroup> GetAll(string search);

        List<OptionGroup> GetAllMapped(string search);

        OptionGroup NewOptionGroup(OptionGroup optionGroup);

        int UpdateOptionGroup(OptionGroup optionGroup);

        OptionGroup GetById(long id);
    }
}
