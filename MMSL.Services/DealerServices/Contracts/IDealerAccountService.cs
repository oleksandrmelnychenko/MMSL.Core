using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.EntityHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.DealerServices.Contracts {
    public interface IDealerAccountService {

        Task<PaginatingResult<DealerAccount>> GetDealerAccounts(int pageNumber, int limit, string searchPhrase, DateTime? from, DateTime? to);
        
        Task<List<DealerAccount>> GetDealerAccountsByPermissionSetting(long prdouctPermissionSettingId);

        Task<DealerAccount> GetDealerAccount(long dealerAccountId);

        Task<long> AddDealerAccount(DealerAccount dealerAccount);

        Task UpdateDealerAccount(DealerAccount dealerAccount);

        Task DeleteDealerAccount(long dealerAccountId);
    }
}
