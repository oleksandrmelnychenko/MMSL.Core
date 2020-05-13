using MMSL.Common.Exceptions.UserExceptions;
using MMSL.Common.IdentityConfiguration;
using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Stores;
using MMSL.Domain.Repositories.Addresses.Contracts;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Common.Helpers;
using MMSL.Services.StoreServices.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.BankDetailsServices {
    public class StoreService : IStoreService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IStoreRepositoriesFactory _storeRepositoriesFactory;

        private readonly IAddressRepositoriesFactory _addressRepositoriesFactory;

        public StoreService(IDbConnectionFactory connectionFactory,
            IStoreRepositoriesFactory storeRepositoriesFactory,
            IAddressRepositoriesFactory addressRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _storeRepositoriesFactory = storeRepositoriesFactory;
            _addressRepositoriesFactory = addressRepositoriesFactory;
        }

        public Task<List<Store>> GetAllStoresAsync() =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    List<Store> stores = null;
                    IStoreRepository bankDetailRepository = _storeRepositoriesFactory.NewStoreRepository(connection);
                    stores = bankDetailRepository.GetAll();
                    return stores;
                }
            });

        public Task<List<Store>> GetAllByDealerStoresAsync(long dealerAccountId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    List<Store> stores = null;
                    IStoreRepository storeRepository = _storeRepositoriesFactory.NewStoreRepository(connection);
                    stores = storeRepository.GetAllByDealerAccountId(dealerAccountId);
                    return stores;
                }
            });

        public Task<Store> NewStoreAsync(NewStoreDataContract newStoreDataContract) =>
             Task.Run(() => {
                 if (!MMSL.Common.Helpers.Validator.IsEmailValid(newStoreDataContract.BillingEmail))
                     UserExceptionCreator<InvalidIdentityException>.Create(
                         IdentityValidationMessages.EMAIL_INVALID, null).Throw();

                 if (!MMSL.Common.Helpers.Validator.IsEmailValid(newStoreDataContract.ContactEmail))
                     UserExceptionCreator<InvalidIdentityException>.Create(
                         IdentityValidationMessages.EMAIL_INVALID, null).Throw();

                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     Store store = null;
                     IStoreRepository storeRepository = _storeRepositoriesFactory.NewStoreRepository(connection);
                     IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);

                     long addressId = addressRepository.AddAddress(newStoreDataContract.Address);

                     Store newStore = new Store {
                         Name = newStoreDataContract.Name,
                         AddressId = addressId,
                         DealerAccountId = newStoreDataContract.DealerAccountId,
                         BillingEmail = newStoreDataContract.BillingEmail,
                         ContactEmail = newStoreDataContract.ContactEmail
                     };

                     store = storeRepository.NewStore(newStore);
                     return store;
                 }
             });

        public Task UpdateStoreAsync(Store store) =>
              Task.Run(() => {
                  using (var connection = _connectionFactory.NewSqlConnection()) {
                      IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);
                      IStoreRepository storeRepository = _storeRepositoriesFactory.NewStoreRepository(connection);

                      if (store.Address != null) {
                          if (store.Address.IsNew()) {
                              long addressId = addressRepository.AddAddress(store.Address);
                              store.AddressId = addressId;
                              store.Address.Id = addressId;
                          } else {
                              addressRepository.UpdateAddress(store.Address);
                          }
                      }

                      storeRepository.UpdateStore(store);
                  }
              });

        public Task DeleteStoreAsunc(long storeId)=>
             Task.Run(() => {
                 using (var connection = _connectionFactory.NewSqlConnection()) {
                     IStoreRepository storeRepository = _storeRepositoriesFactory.NewStoreRepository(connection);

                     Store store = storeRepository.GetStoreById(storeId);

                     if (store == null)
                         throw new System.Exception("Store not found");

                     store.IsDeleted = true;

                     storeRepository.UpdateStore(store);
                 }
             });
    }
}
