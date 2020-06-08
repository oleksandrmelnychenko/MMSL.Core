using MMSL.Domain.Entities.Dealer;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountMapProductPermissionSettingsRepository {
        DealerMapProductPermissionSettings Get(long mapId);
        List<DealerMapProductPermissionSettings> GetByProductPermissionSetting(long productPermissionSettingsId);
        long Add(DealerMapProductPermissionSettings dealerMapProductPermission);
        DealerMapProductPermissionSettings Update(DealerMapProductPermissionSettings dealerMapProductPermission);
    }
}
