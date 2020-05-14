using MMSL.Domain.Entities.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.OptionServices.Contracts {
    public interface IOptionGroupService {

        Task<List<OptionGroup>> GetOptionGroupsAsync();
    }
}
