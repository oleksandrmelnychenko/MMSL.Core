using MMSL.Domain.Repositories.Options.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Options {
    public class OptionRepositoriesFactory : IOptionRepositoriesFactory {
        public IOptionGroupRepository NewOptionGroupRepository(IDbConnection connection) =>
            new OptionGroupRepository(connection);

        public IOptionPriceRepository NewOptionPriceRepository(IDbConnection connection) =>
            new OptionPriceRepository(connection);

        public IOptionUnitRepository NewOptionUnitRepository(IDbConnection connection) =>
            new OptionUnitRepository(connection);

        public IUnitValuesRepository NewUnitValuesRepository(IDbConnection connection) =>
            new UnitValuesRepository(connection);
    }
}
