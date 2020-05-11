using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Services.IdentityServices.Contracts {
    public interface IAccountService {
        Task<IEnumerable<AccountType>> GetAccountTypesByUserIdentity(long userIdentityId, AccountTypes? targetType = null);

        Task<AccountType> AddAccountTypeToUserIdentity(long userIdentityId, AccountType type);

        Task<AccountType> UpdateAccountType(AccountType type);

        Task<AccountType> UpdateAccountTypeAccountInfo(long accountTypeId, AccountTypeCompanyInfo companyInfo);
    }
}
