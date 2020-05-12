using MMSL.Domain.Entities.Dealer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.DealerServices.Contracts {
    public interface IDealerAccountService {

        Task<List<DealerAccount>> GetDealerAccounts();

        Task<DealerAccount> GetDealerAccount(long dealerAccountId);

        Task<long> AddDealerAccount(DealerAccount dealerAccount);

        Task UpdateDealerAccount(DealerAccount dealerAccount);

        Task DeleteDealerAccount(long dealerAccountId);
    }
}
