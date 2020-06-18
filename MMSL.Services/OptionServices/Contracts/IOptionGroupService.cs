using MMSL.Domain.DataContracts.ProductOptions;
using MMSL.Domain.Entities.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices.Contracts {
    public interface IOptionGroupService {

        Task<List<OptionGroup>> GetOptionGroupsAsync(string search, long? productCategoryId = null);

        Task<OptionGroup> GetOptionGroupAsync(long groupId);

        Task<List<OptionGroup>> GetProductOptionGroupsWithPermissionSettingsAsync(long productId, long productSettingsId);

        Task<OptionGroup> NewOptionGroupAsync(NewOptionGroupDataContract newOptionGroupDataContract);

        Task UpdateOptionGroupAsync(OptionGroup optionGroup);

        Task DeleteOptionGroupAsync(long optionGroupId);
    }
}
