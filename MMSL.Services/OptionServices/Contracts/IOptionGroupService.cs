using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices.Contracts {
    public interface IOptionGroupService {

        Task<List<OptionGroup>> GetOptionGroupsAsync();

        Task<OptionGroup> NewOptionGroupAsync(NewOptionGroupDataContract newOptionGroupDataContract);

        Task UpdateOptionGroupAsync(OptionGroup optionGroup);
        Task DeleteOptionGroupAsunc(long optionGroupId);
    }
}
