using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.StoreServices.Contracts {
    public interface IStoreService {
        Task<List<Store>> GetAllStoresAsync();

        Task<Store> NewStore(NewStoreDataContract newStoreDataContract);
    }
}
