using MMSL.Domain.Entities.Dealer;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountRepository {

        List<DealerAccount> GetDealerAccounts();

        DealerAccount GetDealerAccount(long dealerAccountId);

        long AddDealerAccount(DealerAccount dealerAccount);

        void UpdateDealerAccount(DealerAccount dealerAccount);

    }
}
