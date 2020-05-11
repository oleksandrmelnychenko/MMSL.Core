namespace MMSL.Domain.Entities.Identity {
    public class AccountType : EntityBase {

        public long UserIdentityId { get; set; }

        public long? CompanyInfoId { get; set; }

        public AccountTypes UserAccountType { get; set; }

        public UserIdentity UserIdentity { get; set; }

        public AccountTypeCompanyInfo CompanyInfo { get; set; }

    }
}
