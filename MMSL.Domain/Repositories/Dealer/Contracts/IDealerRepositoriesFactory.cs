using System.Data;

namespace MMSL.Domain.Repositories.Dealer.Contracts {
    public interface IDealerRepositoriesFactory {
        IDealerAccountRepository NewDealerAccountRepository(IDbConnection connection);
    }
}
