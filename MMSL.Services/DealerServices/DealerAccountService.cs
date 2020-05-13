﻿using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Addresses.Contracts;
using MMSL.Domain.Repositories.Dealer.Contracts;
using MMSL.Services.DealerServices.Contracts;
using System.Threading.Tasks;

namespace MMSL.Services.DealerServices {
    public class DealerAccountService : IDealerAccountService {

        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDealerRepositoriesFactory _dealerRepositoriesFactory;
        private readonly IAddressRepositoriesFactory _addressRepositoriesFactory;

        public DealerAccountService(IDbConnectionFactory connectionFactory,
            IDealerRepositoriesFactory dealerRepositoriesFactory,
            IAddressRepositoriesFactory addressRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _dealerRepositoriesFactory = dealerRepositoriesFactory;
            _addressRepositoriesFactory = addressRepositoriesFactory;
        }

        public Task<PaginatingResult<DealerAccount>> GetDealerAccounts(int pageNumber, int limit, string searchPhrase) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _dealerRepositoriesFactory
                        .NewDealerAccountRepository(connection)
                        .GetDealerAccounts(pageNumber, limit, searchPhrase);
                }
            });

        public Task<DealerAccount> GetDealerAccount(long dealerAccountId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _dealerRepositoriesFactory
                        .NewDealerAccountRepository(connection)
                        .GetDealerAccount(dealerAccountId);
                }
            });

        public Task<long> AddDealerAccount(DealerAccount dealerAccount) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);

                    if (dealerAccount.BillingAddress != null) {
                        dealerAccount.BillingAddressId = addressRepository.AddAddress(dealerAccount.BillingAddress);
                    }

                    if (dealerAccount.ShippingAddress != null) {
                        dealerAccount.ShippingAddressId = addressRepository.AddAddress(dealerAccount.ShippingAddress);
                    }

                    return _dealerRepositoriesFactory
                        .NewDealerAccountRepository(connection)
                        .AddDealerAccount(dealerAccount);
                }
            });

        public Task UpdateDealerAccount(DealerAccount dealerAccount) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);

                    if (dealerAccount.BillingAddress != null) {
                        addressRepository.UpdateAddress(dealerAccount.BillingAddress);
                    }

                    if (dealerAccount.ShippingAddress != null) {
                        if (dealerAccount.UseBillingAsShipping) {
                            dealerAccount.ShippingAddressId = dealerAccount.BillingAddressId;

                            dealerAccount.ShippingAddress.IsDeleted = true;
                        }

                        addressRepository.UpdateAddress(dealerAccount.ShippingAddress);
                    }

                    _dealerRepositoriesFactory
                        .NewDealerAccountRepository(connection)
                        .UpdateDealerAccount(dealerAccount);
                }
            });

        public Task DeleteDealerAccount(long dealerAccountId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IDealerAccountRepository dealerrepository = _dealerRepositoriesFactory.NewDealerAccountRepository(connection);

                    DealerAccount dealerAccount = dealerrepository.GetDealerAccount(dealerAccountId);

                    if (dealerAccount == null)
                        throw new System.Exception("Dealer not found");

                    dealerAccount.IsDeleted = true;

                    dealerrepository.UpdateDealerAccount(dealerAccount);
                }
            });
    }
}