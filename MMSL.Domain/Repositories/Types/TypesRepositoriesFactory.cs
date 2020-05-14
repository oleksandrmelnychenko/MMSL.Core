using MMSL.Domain.Repositories.Types.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Types {
    public class TypesRepositoriesFactory : ITypesRepositoriesFactory {
        public ICurrencyTypeRepository NewCurrencyTypeRepository(IDbConnection connection) => 
            new CurrencyTypeRepository(connection);

        public IPaymentTypeRepository NewPaymentTypeRepository(IDbConnection connection) =>
            new PaymentTypeRepository(connection);
    }
}
