using MMSL.Domain.Entities.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.Stores.Contracts {
    public interface IStoreRepository {
        List<Store> GetAll();
    }
}
