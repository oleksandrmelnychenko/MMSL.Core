﻿using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices.Contracts {
    public interface IOptionGroupService {

        Task<List<OptionGroup>> GetOptionGroupsAsync(string search);

        Task<OptionGroup> GetOptionGroupAsync(long groupId);

        Task<OptionGroup> NewOptionGroupAsync(NewOptionGroupDataContract newOptionGroupDataContract);

        Task UpdateOptionGroupAsync(OptionGroup optionGroup);

        Task DeleteOptionGroupAsync(long optionGroupId);
    }
}
