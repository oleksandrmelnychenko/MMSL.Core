using MMSL.Domain.Entities.Dealer;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountRepository {

        DealerAccount GetDealerAccount(long dealerAccountId);

        long AddDealerAccount(DealerAccount dealerAccount);

        void UpdateDealerAccount(DealerAccount dealerAccount);

    }
}
