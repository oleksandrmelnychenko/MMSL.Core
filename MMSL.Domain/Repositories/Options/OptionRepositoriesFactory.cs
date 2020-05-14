using MMSL.Domain.Repositories.Options.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Options {
    public class OptionRepositoriesFactory : IOptionRepositoriesFactory {
        public IOptionGroupRepository NewOptionGroupRepository(IDbConnection connection) =>
            new OptionGroupRepository(connection);

        public IOptionUnitRepository NewOptionUnitRepository(IDbConnection connection) =>
            new OptionUnitRepository(connection);
    }
}
