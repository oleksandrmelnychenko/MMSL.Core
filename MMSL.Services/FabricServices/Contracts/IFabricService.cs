using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.Entities.Fabrics;
using System.Threading.Tasks;

namespace MMSL.Services.FabricServices.Contracts {
    public interface IFabricService {
        Task<Fabric> GetByIdAsync(long fabricId);
        Task<Fabric> AddFabric(NewFabricDataContract fabric, string imageUrl = null);
        Task<Fabric> UpdateFabric(UpdateFabricDataContract fabric, string newImageUrl = null);
        Task<Fabric> DeleteFabric(long fabricId);
        Task<Fabric> UpdateFabricVisibilities(UpdateFabricVisibilitiesDataContract fabric);
    }
}
