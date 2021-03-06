﻿using MMSL.Common.Exceptions;
using MMSL.Common.Exceptions.DealerExceptions;
using MMSL.Common.Helpers;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Addresses.Contracts;
using MMSL.Domain.Repositories.Dealer.Contracts;
using MMSL.Services.DealerServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<PaginatingResult<DealerAccount>> GetDealerAccounts(int pageNumber, int limit, string searchPhrase, DateTime? from, DateTime? to) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _dealerRepositoriesFactory
                        .NewDealerAccountRepository(connection)
                        .GetDealerAccounts(pageNumber, limit, searchPhrase, from, to);
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

        public Task<DealerAccount> AddDealerAccount(DealerAccount dealerAccount) =>
            Task.Run(() => {
                ValidateDealerAccount(dealerAccount);

                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);
                    IDealerAccountRepository dealerRepository = _dealerRepositoriesFactory.NewDealerAccountRepository(connection);

                    if (dealerAccount.BillingAddress != null && dealerAccount.BillingAddress.IsNew()) {
                        dealerAccount.BillingAddressId = dealerAccount.BillingAddress.Id = addressRepository.AddAddress(dealerAccount.BillingAddress);
                    }

                    if (dealerAccount.ShippingAddress != null && dealerAccount.ShippingAddress.IsNew()) {
                        dealerAccount.ShippingAddressId = dealerAccount.ShippingAddress.Id = addressRepository.AddAddress(dealerAccount.ShippingAddress);
                    }

                    long id = dealerRepository.AddDealerAccount(dealerAccount);

                    return dealerRepository.GetDealerAccount(id);
                }
            });

        public Task<DealerAccount> UpdateDealerAccount(DealerAccount dealerAccount) =>
            Task.Run(() => {
                ValidateDealerAccount(dealerAccount);

                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IAddressRepository addressRepository = _addressRepositoriesFactory.NewAddressRepository(connection);
                    IDealerAccountRepository dealerRepository = _dealerRepositoriesFactory.NewDealerAccountRepository(connection);

                    DealerAccount existingDealer = dealerRepository.GetDealerAccount(dealerAccount.Id);

                    if (existingDealer == null)
                        ExceptionCreator<DealerNotFoundException>.Create("Dealer not found")
                            .Throw();

                    if (dealerAccount.BillingAddress != null) {
                        if (dealerAccount.BillingAddress.IsNew()) {
                            dealerAccount.BillingAddressId = addressRepository.AddAddress(dealerAccount.BillingAddress);
                        } else {
                            addressRepository.UpdateAddress(dealerAccount.BillingAddress);
                        }
                    }

                    if (dealerAccount.ShippingAddress != null) {
                        if (dealerAccount.ShippingAddress.IsNew()) {
                            dealerAccount.ShippingAddressId = addressRepository.AddAddress(dealerAccount.ShippingAddress);
                        } else {
                            if (dealerAccount.UseBillingAsShipping) {
                                dealerAccount.ShippingAddressId = dealerAccount.BillingAddressId;

                                dealerAccount.ShippingAddress.IsDeleted = true;
                            }

                            addressRepository.UpdateAddress(dealerAccount.ShippingAddress);
                        }
                    }

                    dealerRepository.UpdateDealerAccount(dealerAccount);

                    return dealerRepository.GetDealerAccount(dealerAccount.Id);
                }
            });

        public Task DeleteDealerAccount(long dealerAccountId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IDealerAccountRepository dealerrepository = _dealerRepositoriesFactory.NewDealerAccountRepository(connection);

                    DealerAccount dealerAccount = dealerrepository.GetDealerAccount(dealerAccountId);

                    if (dealerAccount == null)
                        ExceptionCreator<DealerNotFoundException>.Create("Dealer not found")
                            .Throw();

                    dealerAccount.IsDeleted = true;

                    dealerrepository.UpdateDealerAccount(dealerAccount);
                }
            });

        public Task<List<DealerAccount>> GetDealerAccountsByPermissionSetting(long prdouctPermissionSettingId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _dealerRepositoriesFactory
                        .NewDealerAccountRepository(connection)
                        .GetDealerAccounts(prdouctPermissionSettingId);
                }
            });

        public Task<List<DealerAccount>> SearchDealerAccountsByPermissionSetting(string searchPhrase, long? productId, bool? excludeMatchPermission) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _dealerRepositoriesFactory
                        .NewDealerAccountRepository(connection)
                        .SearchDealerAccounts(searchPhrase, productId, excludeMatchPermission);
                }
            });

        private void ValidateDealerAccount(DealerAccount dealerAccount) {
            if (string.IsNullOrEmpty(dealerAccount.Email))
                ExceptionCreator<InvalidDealerModelException>.Create("Email is required")
                    .Throw();

            if (!Validator.IsEmailValid(dealerAccount.Email))
                ExceptionCreator<InvalidDealerModelException>.Create("Dealer email is invalid")
                    .Throw();

            if (string.IsNullOrEmpty(dealerAccount.AlternateEmail))
                ExceptionCreator<InvalidDealerModelException>.Create("Alternate email is required")
                    .Throw();

            if (!Validator.IsEmailValid(dealerAccount.AlternateEmail))
                ExceptionCreator<InvalidDealerModelException>.Create("Dealer alternate email is invalid")
                    .Throw();

            if (string.IsNullOrEmpty(dealerAccount.CompanyName))
                ExceptionCreator<InvalidDealerModelException>.Create("Company name is required")
                    .Throw();

            if (string.IsNullOrEmpty(dealerAccount.Name))
                ExceptionCreator<InvalidDealerModelException>.Create("Dealer name is required")
                    .Throw();
        }
    }
}
