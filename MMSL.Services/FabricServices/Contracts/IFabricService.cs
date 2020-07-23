using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.DataContracts.Filters;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.EntityHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.FabricServices.Contracts {
    public interface IFabricService {
        Task<Fabric> GetByIdAsync(long fabricId);
        Task<Fabric> AddFabric(NewFabricDataContract fabric, long userIdentityId, string imageUrl = null);
        Task<Fabric> UpdateFabric(UpdateFabricDataContract fabric, string newImageUrl = null);
        Task<Fabric> DeleteFabric(long fabricId);
        Task UpdateFabricVisibilities(UpdateFabricVisibilitiesDataContract fabric, long userIdentityId);
        Task<PaginatingResult<Fabric>> GetFabrics(int pageNumber, int limit, string searchPhrase, FilterItem[] filters, long? ownerUserIdentityId);
        Task<List<FilterItem>> GetFabricFilters();
        Task<string> PrepareFabricsPdf(string searchPhrase, FilterItem[] filters);
    }
}
