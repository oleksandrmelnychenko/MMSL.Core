using MMSL.Domain.Entities.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface IStoreRepository {
        List<Store> GetAll();

        Store NewStore(Store store);

        void UpdateStore(Store store);

        Store GetStoreById(long storeId);

        List<Store> GetAllByDealerAccountId(long dealerAccountId);
    }
}
