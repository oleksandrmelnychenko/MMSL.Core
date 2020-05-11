using MMSL.Domain.Entities.Identity;

namespace MMSL.Domain.Repositories.Identity.Contracts {
    public interface IAccountTypeCompanyInfoRepository {
        AccountTypeCompanyInfo GetAccountTypeCompanyInfo(long companyInfoId);
        long AddAccountTypeCompanyInfo(AccountTypeCompanyInfo companyInfo);
        void UpdateAccountTypeCompanyInfo(AccountTypeCompanyInfo companyInfo);
    }
}
