﻿using MMSL.Domain.Entities.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface IStoreRepository {
        List<Store> GetAll(string searchPhrase);

        Store NewStore(Store store, long dealerAccountId);

        void UpdateStore(Store store);

        Store GetStoreById(long storeId);

        List<Store> GetAllByDealerAccountId(long dealerAccountId, string searchPhrase);
    }
}
