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
                    List<Store> bankDetails = null;
                    IStoreRepository bankDetailRepository = _storeRepositoriesFactory.NewStoreRepository(connection);
                    bankDetails = bankDetailRepository.GetAll();
                    return bankDetails;
                }
            });

        public Task<Store> NewStore(NewStoreDataContract newStoreDataContract) =>
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
    }
}
