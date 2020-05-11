using System.Collections.Generic;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Domain.Repositories.Identity.Contracts {
    public interface IAccountTypeRepository {
        IEnumerable<AccountType> GetAccountTypes(long accountTypeId, AccountTypes? targetType = null);
        AccountType GetAccountType(long userIdentityId);
        long AddAccountType(AccountType accountType);
        void UpdateAccountType(AccountType accountType);
    }
}
