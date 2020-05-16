using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Addresses.Contracts;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Services.StoreCustomerServices.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.StoreCustomerServices {
    public class StoreCustomerService : IStoreCustomerService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IStoreRepositoriesFactory _storeRepositoriesFactory;

        private readonly IAddressRepositoriesFactory _addressRepositoriesFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        public StoreCustomerService(IDbConnectionFactory connectionFactory, IStoreRepositoriesFactory storeRepositoriesFactory, IAddressRepositoriesFactory addressRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _storeRepositoriesFactory = storeRepositoriesFactory;
            _addressRepositoriesFactory = addressRepositoriesFactory;
        }

        public Task<StoreCustomer> AddCustomerAsync(StoreCustomer storeCustomer) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IStoreCustomerRepository storeCustomerRepository = _storeRepositoriesFactory.NewStoreCustomerRepository(connection);
                    IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);

                    if (storeCustomer.BillingAddress != null) {
                        storeCustomer.BillingAddressId = addressRepository.AddAddress(storeCustomer.BillingAddress);
                        storeCustomer.BillingAddress.Id = storeCustomer.BillingAddressId.Value;
                    } else {
                        storeCustomer.BillingAddressId = null;
                    }

                    if (!storeCustomer.UseBillingAsDeliveryAddress &&
                        storeCustomer.DeliveryAddress != null) {

                        storeCustomer.DeliveryAddressId = addressRepository.AddAddress(storeCustomer.DeliveryAddress);
                    } else {
                        storeCustomer.DeliveryAddressId = null;
                    }

                    storeCustomer.Id = storeCustomerRepository.AddStoreCustomer(storeCustomer);

                    return storeCustomer;
                }
            });

        public Task<StoreCustomer> DeleteCustomerAsync(long storeCustomerId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IStoreCustomerRepository storeCustomerRepository = _storeRepositoriesFactory.NewStoreCustomerRepository(connection);

                    StoreCustomer customer = storeCustomerRepository.GetStoreCustomer(storeCustomerId);

                    customer.IsDeleted = true;

                    storeCustomerRepository.UpdateStoreCustomer(customer);

                    return customer;
                }
            });

        public Task<StoreCustomer> GetCustomerAsync(long storeCustomerId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _storeRepositoriesFactory
                        .NewStoreCustomerRepository(connection)
                        .GetStoreCustomer(storeCustomerId);
                }
            });

        public Task<PaginatingResult<StoreCustomer>> GetCustomersByStoreAsync(int pageNumber, int limit, string searchPhrase, string storeName, long? storeId = null) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _storeRepositoriesFactory
                        .NewStoreCustomerRepository(connection)
                        .GetStoreCustomers(pageNumber, limit, searchPhrase, storeName, storeId);
                }
            });

        public Task<StoreCustomer> UpdateCustomerAsync(StoreCustomer storeCustomer) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {

                    if (storeCustomer.IsNew()) {
                        //TODO: throw argexc id 
                    }

                    IStoreCustomerRepository storeCustomerRepository = _storeRepositoriesFactory.NewStoreCustomerRepository(connection);
                    IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);

                    if (storeCustomer.BillingAddress != null) {
                        if (storeCustomer.BillingAddress.IsNew()) {
                            storeCustomer.BillingAddressId = addressRepository.AddAddress(storeCustomer.BillingAddress);
                        } else {
                            addressRepository.UpdateAddress(storeCustomer.BillingAddress);    
                        }
                    }

                    if (!storeCustomer.UseBillingAsDeliveryAddress && storeCustomer.DeliveryAddress != null) {
                        if (storeCustomer.DeliveryAddress.IsNew()) {
                            storeCustomer.DeliveryAddressId = addressRepository.AddAddress(storeCustomer.DeliveryAddress);
                        } else {
                            addressRepository.UpdateAddress(storeCustomer.DeliveryAddress);
                        }
                    }

                    storeCustomerRepository.UpdateStoreCustomer(storeCustomer);

                    return storeCustomer;
                }
            });
    }
}
