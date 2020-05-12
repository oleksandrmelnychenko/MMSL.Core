using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMSL.Domain.Repositories.BankDetails.Contracts {
    public interface IBankDetailRepositoriesFactory {
        IBankDetailRepository NewBankDetailRepository(IDbConnection connection);
    }
}
