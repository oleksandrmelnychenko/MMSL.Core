using System.Data;

namespace MMSL.Domain.Repositories.Types.Contracts {
    public interface ITypesRepositoriesFactory {
        ICurrencyTypeRepository NewCurrencyTypeRepository(IDbConnection connection);
        IPaymentTypeRepository NewPaymentTypeRepository(IDbConnection connection);
    }
}
