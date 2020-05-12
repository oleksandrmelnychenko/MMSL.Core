using MMSL.Domain.Entities.Dealer;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountRepository {

        DealerAccount GetAddress(long dealerAccountId);

        long AddAddress(DealerAccount dealerAccount);

        void UpdateAddress(DealerAccount dealerAccount);

    }
}
