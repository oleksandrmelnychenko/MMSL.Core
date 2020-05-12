using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Stores;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Services.StoreServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.BankDetailsServices {
    public class StoreService : IStoreService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IStoreRepositoriesFactory _bankDetailRepositoriesFactory;

        public StoreService(IDbConnectionFactory connectionFactory, IStoreRepositoriesFactory bankDetailRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _bankDetailRepositoriesFactory = bankDetailRepositoriesFactory;
        }

        public Task<List<Store>> GetAllStoresAsync() =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    List<Store> bankDetails = null;
                    IStoreRepository bankDetailRepository = _bankDetailRepositoriesFactory.NewBankDetailRepository(connection);
                    bankDetails = bankDetailRepository.GetAll();
                    return bankDetails;
                }
            });

        public Task<Store> NewStore(NewStoreDataContract newStoreDataContract) {
            throw new NotImplementedException();
        }
    }
}
