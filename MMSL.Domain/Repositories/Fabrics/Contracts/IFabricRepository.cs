using MMSL.Domain.Entities.Fabrics;

namespace MMSL.Domain.Repositories.Fabrics.Contracts {
    public interface IFabricRepository {
        Fabric GetById(long fabricId);
        Fabric GetByIdForDealer(long fabricId);
        Fabric UpdateFabric(Fabric fabricEntity);
        Fabric AddFabric(Fabric fabricEntity);
    }
}
