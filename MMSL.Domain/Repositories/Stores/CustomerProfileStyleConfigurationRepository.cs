using Dapper;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.Repositories.Stores.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.Stores {
    class CustomerProfileStyleConfigurationRepository : ICustomerProfileStyleConfigurationRepository {

        private readonly IDbConnection _connection;

        public CustomerProfileStyleConfigurationRepository(IDbConnection connection) {
            _connection = connection;
        }

        public CustomerProfileStyleConfiguration GetById(long id) =>
            _connection.QuerySingleOrDefault<CustomerProfileStyleConfiguration>(
                "SELECT [CustomerProfileStyleConfigurations].* " +
                "FROM [CustomerProfileStyleConfigurations] " +
                "AND [CustomerProfileStyleConfigurations].Id = @Id",
                new { Id = id });

        public long Add(CustomerProfileStyleConfiguration styleConfiguration) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [CustomerProfileStyleConfigurations] (IsDeleted,[OptionUnitId],[CustomerProductProfileId],[UnitValueId])" +
                "VALUES(@IsDeleted, @OptionUnitId, @CustomerProductProfileId, @UnitValueId)" +
                "SELECT SCOPE_IDENTITY()",
                styleConfiguration);

        public CustomerProfileStyleConfiguration Update(CustomerProfileStyleConfiguration styleConfiguration) =>
            _connection.QuerySingleOrDefault<CustomerProfileStyleConfiguration>(
                "UPDATE [CustomerProfileStyleConfigurations] " +
                "SET LastModified = GETUTCDATE(), [IsDeleted] = @IsDeleted," +
                "[OptionUnitId] = @OptionUnitId, [UnitValueId] = @UnitValueId " +
                "WHERE [CustomerProfileStyleConfigurations].Id = @Id",
                styleConfiguration);
    }
}
