using MMSL.Domain.Entities.Dealer;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountMapProductPermissionSettingsRepository {
        DealerMapProductPermissionSettings Get(long mapId);
        long Add(DealerMapProductPermissionSettings dealerMapProductPermission);
        DealerMapProductPermissionSettings Update(DealerMapProductPermissionSettings dealerMapProductPermission);
    }
}
