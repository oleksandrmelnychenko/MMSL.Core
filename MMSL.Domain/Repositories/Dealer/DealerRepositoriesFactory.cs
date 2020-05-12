﻿using MMSL.Domain.Repositories.Dealer.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Dealer {
    public class DealerRepositoriesFactory : IDealerRepositoriesFactory {
        public IDealerAccountRepository NewDealerAccountRepository(IDbConnection connection) =>
            new DealerAccountRepository(connection);
    }
}
