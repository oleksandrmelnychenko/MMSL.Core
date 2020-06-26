﻿using MMSL.Domain.DataContracts.Customer;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Services.StoreCustomerServices.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.StoreCustomerServices {
    public class StoreCustomerProductProfileService : IStoreCustomerProductProfileService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IStoreRepositoriesFactory _storeRepositoriesFactory;

        public StoreCustomerProductProfileService(IDbConnectionFactory connectionFactory, IStoreRepositoriesFactory storeRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _storeRepositoriesFactory = storeRepositoriesFactory;
        }

        public Task<CustomerProductProfile> GetByIdAsync(long profileId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _storeRepositoriesFactory
                        .NewCustomerProductProfileRepository(connection)
                        .GetCustomerProductProfile(profileId);
                }
            });

        public Task<List<CustomerProductProfile>> GetAllAsync(long dealerIdentityId, long? productCategoryId, string searchPhrase = null) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _storeRepositoriesFactory
                        .NewCustomerProductProfileRepository(connection)
                        .GetCustomerProductProfilesByDealerIdentity(dealerIdentityId, productCategoryId);
                }
            });

        public Task<CustomerProductProfile> AddAsync(long dealerIdentityId, NewCustomerProductProfile newProfileDataContract) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    ICustomerProductProfileRepository profileRepository = _storeRepositoriesFactory.NewCustomerProductProfileRepository(connection);
                    ICustomerProfileSizeValueRepository profileValueRepository = _storeRepositoriesFactory.NewCustomerProfileSizeValueRepository(connection);

                    CustomerProductProfile entity = newProfileDataContract.GetEntity();
                    entity.Id = profileRepository.AddCustomerProductProfile(entity);

                    foreach (NewCustomerProfileValueDataContract item in newProfileDataContract.Values) {
                        CustomerProfileSizeValue newValue = new CustomerProfileSizeValue() {
                            MeasurementDefinitionId = item.MeasurementDefinitionId,
                            Value = item.Value,
                            FittingValue = item.FittingValue
                        };

                        profileValueRepository.AddSizeValue(newValue);
                    }

                    return profileRepository.GetCustomerProductProfile(entity.Id);
                }
            });

        public Task<CustomerProductProfile> UpdateAsync(UpdateCustomerProductProfile profileDataContract) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    ICustomerProductProfileRepository profileRepository = _storeRepositoriesFactory.NewCustomerProductProfileRepository(connection);
                    ICustomerProfileSizeValueRepository profileValueRepository = _storeRepositoriesFactory.NewCustomerProfileSizeValueRepository(connection);

                    CustomerProductProfile entity = profileDataContract.GetEntity();
                    entity = profileRepository.UpdateCustomerProductProfile(entity);

                    foreach (UpdateCustomerProfileValueDataContract item in profileDataContract.Values) {
                        CustomerProfileSizeValue valueToUpdate = item.GetEntity();

                        if (valueToUpdate.IsNew()) {
                            profileValueRepository.AddSizeValue(valueToUpdate);
                        } else {
                            profileValueRepository.UpdateSizeValue(valueToUpdate);
                        }
                    }

                    return profileRepository.GetCustomerProductProfile(entity.Id);
                }
            });

    }
}
