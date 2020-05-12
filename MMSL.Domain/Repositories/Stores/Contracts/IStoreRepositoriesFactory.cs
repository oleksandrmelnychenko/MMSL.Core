using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface IStoreRepositoriesFactory {
        IStoreRepository NewStoreRepository(IDbConnection connection);
    }
}
