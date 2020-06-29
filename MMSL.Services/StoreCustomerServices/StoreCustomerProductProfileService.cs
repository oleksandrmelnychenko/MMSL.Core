using MMSL.Domain.DataContracts.Customer;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.Repositories.Dealer.Contracts;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Services.StoreCustomerServices.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MMSL.Services.StoreCustomerServices {
    public class StoreCustomerProductProfileService : IStoreCustomerProductProfileService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IStoreRepositoriesFactory _storeRepositoriesFactory;

        private readonly IDealerRepositoriesFactory _dealerRepositoriesFactory;
        
        public StoreCustomerProductProfileService(
            IDbConnectionFactory connectionFactory, 
            IDealerRepositoriesFactory dealerRepositoriesFactory,
            IStoreRepositoriesFactory storeRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _storeRepositoriesFactory = storeRepositoriesFactory;
            _dealerRepositoriesFactory = dealerRepositoriesFactory;
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
                    IDealerAccountRepository dealerrepository = _dealerRepositoriesFactory.NewDealerAccountRepository(connection);
                    ICustomerProfileStyleConfigurationRepository styleConfigRepository = _storeRepositoriesFactory.NewCustomerProfileStyleConfigurationRepository(connection);

                    DealerAccount dealer = dealerrepository.GetDealerAccountByIdentity(dealerIdentityId);

                    CustomerProductProfile entity = newProfileDataContract.GetEntity();
                    entity.DealerAccountId = dealer.Id;

                    entity.Id = profileRepository.AddCustomerProductProfile(entity);

                    foreach (NewCustomerProfileValueDataContract item in newProfileDataContract.Values) {
                        CustomerProfileSizeValue newValue = new CustomerProfileSizeValue() {
                            MeasurementDefinitionId = item.MeasurementDefinitionId,
                            Value = item.Value,
                            FittingValue = item.FittingValue
                        };

                        profileValueRepository.AddSizeValue(newValue);
                    }

                    foreach (NewProfileProductStyleConfigurationDataContract styleDataContract in newProfileDataContract.ProductStyles) {
                        styleConfigRepository.Add(new CustomerProfileStyleConfiguration {
                            SelectedValue = styleDataContract.SelectedStyleValue,
                            OptionUnitId = styleDataContract.OptionUnitId
                        });
                    }

                    return profileRepository.GetCustomerProductProfile(entity.Id);
                }
            });

        public Task<CustomerProductProfile> UpdateAsync(UpdateCustomerProductProfile profileDataContract) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    ICustomerProductProfileRepository profileRepository = _storeRepositoriesFactory.NewCustomerProductProfileRepository(connection);
                    ICustomerProfileSizeValueRepository profileValueRepository = _storeRepositoriesFactory.NewCustomerProfileSizeValueRepository(connection);
                    ICustomerProfileStyleConfigurationRepository styleConfigRepository = _storeRepositoriesFactory.NewCustomerProfileStyleConfigurationRepository(connection);

                    CustomerProductProfile entity = profileDataContract.GetEntity();
                    entity = profileRepository.UpdateCustomerProductProfile(entity);

                    foreach (UpdateCustomerProfileValueDataContract item in profileDataContract.Values) {
                        CustomerProfileSizeValue valueToUpdate = item.GetEntity();
                        valueToUpdate.CustomerProductProfileId = entity.Id;

                        if (valueToUpdate.IsNew()) {
                            profileValueRepository.AddSizeValue(valueToUpdate);
                        } else {
                            profileValueRepository.UpdateSizeValue(valueToUpdate);
                        }
                    }

                    foreach (UpdateProfileProductStyleConfigurationDataContract styleDataContract in profileDataContract.ProductStyles) {
                        CustomerProfileStyleConfiguration styleEntity = new CustomerProfileStyleConfiguration {
                            Id = styleDataContract.Id,
                            IsDeleted = styleDataContract.IsDeleted,
                            SelectedValue = styleDataContract.SelectedStyleValue,
                            OptionUnitId = styleDataContract.OptionUnitId,
                            CustomerProductProfileId = entity.Id
                        };

                        if (styleEntity.IsNew()) {
                            styleConfigRepository.Add(styleEntity);
                        } else {
                            styleConfigRepository.Update(styleEntity);
                        }
                    }

                    return profileRepository.GetCustomerProductProfile(profileDataContract.Id);
                }
            });

        public Task<CustomerProductProfile> DeleteAsync(long customerProductProfileId) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    ICustomerProductProfileRepository profileRepository = _storeRepositoriesFactory.NewCustomerProductProfileRepository(connection);
                    ICustomerProfileSizeValueRepository profileValueRepository = _storeRepositoriesFactory.NewCustomerProfileSizeValueRepository(connection);

                    CustomerProductProfile entity = profileRepository.GetCustomerProductProfile(customerProductProfileId);

                    entity.IsDeleted = true;

                    profileRepository.UpdateCustomerProductProfile(entity);

                    return entity;
                }
            });
    }
}
