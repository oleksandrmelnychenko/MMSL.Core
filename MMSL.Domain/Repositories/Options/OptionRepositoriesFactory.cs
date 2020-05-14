using MMSL.Domain.Repositories.Options.Contracts;
using System;
using System.Data;

namespace MMSL.Domain.Repositories.Options {
    public class OptionRepositoriesFactory : IOptionRepositoriesFactory {
        public IOptionUnitRepository NewOptionUnitRepository(IDbConnection connection) =>
            new OptionUnitRepository(connection);
    }
}
