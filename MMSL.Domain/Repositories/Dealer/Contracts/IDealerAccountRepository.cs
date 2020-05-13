using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.EntityHelpers;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerAccountRepository {

        PaginatingResult<DealerAccount> GetDealerAccounts(int pageNumber, int limit, string searchPhrase);

        DealerAccount GetDealerAccount(long dealerAccountId);

        long AddDealerAccount(DealerAccount dealerAccount);

        void UpdateDealerAccount(DealerAccount dealerAccount);

    }
}
