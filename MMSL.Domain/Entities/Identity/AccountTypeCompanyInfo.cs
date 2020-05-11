using System.Collections.Generic;

namespace MMSL.Domain.Entities.Identity {
    public class AccountTypeCompanyInfo : EntityBaseNamed {
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<AccountType> AccountTypes { get; set; }
    }
}
