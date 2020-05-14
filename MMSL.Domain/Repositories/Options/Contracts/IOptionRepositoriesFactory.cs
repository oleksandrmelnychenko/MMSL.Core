using System.Data;

namespace MMSL.Domain.Repositories.Options.Contracts {
    public interface IOptionRepositoriesFactory {
        IOptionUnitRepository NewOptionUnitRepository(IDbConnection connection);

        IOptionGroupRepository NewOptionGroupRepository(IDbConnection connection);
    }
}
