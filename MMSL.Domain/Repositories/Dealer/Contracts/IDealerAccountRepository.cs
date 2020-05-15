using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.EntityHelpers;
using System;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountRepository {

        PaginatingResult<DealerAccount> GetDealerAccounts(int pageNumber, int limit, string searchPhrase, DateTime? from, DateTime? to);

        DealerAccount GetDealerAccount(long dealerAccountId);

        long AddDealerAccount(DealerAccount dealerAccount);

        void UpdateDealerAccount(DealerAccount dealerAccount);

    }
}
