using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvested.AI.Services.IdentityServices.Contracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.Repositories.Identity.Contracts;
using MMSL.Services.IdentityServices.Contracts;

namespace MMSL.Services.IdentityServices {
    public class AccountService : IAccountService {
        private readonly IIdentityRepositoriesFactory _identityRepositoriesFactory;
        private readonly IDbConnectionFactory _connectionFactory;

        public AccountService(IDbConnectionFactory connectionFactory,
            IIdentityRepositoriesFactory identityRepositoriesFactory)
        {

            _identityRepositoriesFactory = identityRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<Domain.Entities.Identity.AccountType> AddAccountTypeToUserIdentity(long userIdentityId, Domain.Entities.Identity.AccountType type) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IAccountTypeRepository repository = _identityRepositoriesFactory.NewAccountTypeRepository(connection);
                    type.UserIdentityId = userIdentityId;

                    long createdEntityId = repository.AddAccountType(type);

                    return repository.GetAccountType(createdEntityId);
                }
            });

        public Task<IEnumerable<Domain.Entities.Identity.AccountType>> GetAccountTypesByUserIdentity(long userIdentityId, AccountTypes? targetType = null) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _identityRepositoriesFactory
                        .NewAccountTypeRepository(connection)
                        .GetAccountTypes(userIdentityId, targetType);
                }
            });

        public Task<Domain.Entities.Identity.AccountType> UpdateAccountType(Domain.Entities.Identity.AccountType type) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    var typeRepository = _identityRepositoriesFactory.NewAccountTypeRepository(connection);
                    var infoRepository = _identityRepositoriesFactory.NewAccountTypeCompanyInfoRepository(connection);

                    if (type.CompanyInfo != null) {
                        if (type.CompanyInfo.IsNew()) {
                            type.CompanyInfoId = infoRepository.AddAccountTypeCompanyInfo(type.CompanyInfo);
                        } else {
                            infoRepository.UpdateAccountTypeCompanyInfo(type.CompanyInfo);
                        }
                    }

                    typeRepository.UpdateAccountType(type);

                    return typeRepository.GetAccountType(type.Id);
                }
            });

        public Task<Domain.Entities.Identity.AccountType> UpdateAccountTypeAccountInfo(long accountTypeId, AccountTypeCompanyInfo companyInfo) =>
            Task.Run(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    var typeRepository = _identityRepositoriesFactory.NewAccountTypeRepository(connection);
                    var infoRepository = _identityRepositoriesFactory.NewAccountTypeCompanyInfoRepository(connection);

                    if (companyInfo != null) {
                        if (companyInfo.IsNew()) {
                            Domain.Entities.Identity.AccountType targetType = typeRepository.GetAccountType(accountTypeId);

                            targetType.CompanyInfoId = infoRepository.AddAccountTypeCompanyInfo(companyInfo);

                            typeRepository.UpdateAccountType(targetType);
                        } else {
                            infoRepository.UpdateAccountTypeCompanyInfo(companyInfo);
                        }
                    }

                    return typeRepository.GetAccountType(accountTypeId);
                }
            });

    }
}
