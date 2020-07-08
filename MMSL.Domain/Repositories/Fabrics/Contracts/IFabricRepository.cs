using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.EntityHelpers;

namespace MMSL.Domain.Repositories.Fabrics.Contracts {
    public interface IFabricRepository {
        Fabric GetById(long fabricId);
        Fabric GetByIdForDealer(long fabricId);
        Fabric UpdateFabric(Fabric fabricEntity);
        Fabric AddFabric(Fabric fabricEntity);
        PaginatingResult<Fabric> GetPagination(int pageNumber, int limit, string searchPhrase);
    }
}
