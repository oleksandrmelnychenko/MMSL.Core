using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.EntityHelpers;
using System;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountRepository {

        PaginatingResult<DealerAccount> GetDealerAccounts(int pageNumber, int limit, string searchPhrase, DateTime? from, DateTime? to);

        List<DealerAccount> GetDealerAccounts(long productPermissionSettingId);

        List<DealerAccount> SearchDealerAccounts(string searchPhrase, long? productId = null, bool? excludeMatchPermission = null);

        DealerAccount GetDealerAccount(long dealerAccountId);

        long AddDealerAccount(DealerAccount dealerAccount);

        void UpdateDealerAccount(DealerAccount dealerAccount);

    }
}
