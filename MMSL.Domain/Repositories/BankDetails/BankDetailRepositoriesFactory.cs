using MMSL.Domain.Repositories.BankDetails.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.BankDetails {
    public class BankDetailRepositoriesFactory : IBankDetailRepositoriesFactory {
        public IBankDetailRepository NewBankDetailRepository(IDbConnection connection)
            => new BankDetailRepository(connection);
    }
}
