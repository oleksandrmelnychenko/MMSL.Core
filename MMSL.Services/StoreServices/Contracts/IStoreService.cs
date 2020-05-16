using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.StoreServices.Contracts {
    public interface IStoreService {
        Task<List<Store>> GetAllStoresAsync(string searchPhrase);

        Task<Store> NewStoreAsync(NewStoreDataContract newStoreDataContract);

        Task UpdateStoreAsync(Store store);

        Task DeleteStoreAsunc(long storeId);

        Task<List<Store>> GetAllByDealerStoresAsync(long dealerAccountId, string searchPhrase);
    }
}
