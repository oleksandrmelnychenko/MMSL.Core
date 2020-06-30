using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.Repositories.Stores.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Stores {
    class CustomerProfileSizeValueRepository : ICustomerProfileSizeValueRepository {

        private readonly IDbConnection _connection;

        public CustomerProfileSizeValueRepository(IDbConnection connection) {
            _connection = connection;
        }

        public CustomerProfileSizeValue GetSizeValue(long sizeValueId) =>
            _connection.Query<CustomerProfileSizeValue, MeasurementDefinition, CustomerProfileSizeValue>(
@"SELECT [CustomerProfileSizeValues].*,[MeasurementDefinitions].*
FROM [CustomerProfileSizeValues]
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [CustomerProfileSizeValues].MeasurementDefinitionId
WHERE [CustomerProfileSizeValues].Id = @Id",
                (value, dehinition) => {
                    value.MeasurementDefinition = dehinition;
                    return value;
                },
                new { Id = sizeValueId })
            .SingleOrDefault();

        public long AddSizeValue(CustomerProfileSizeValue sizeValue) =>
            _connection.QuerySingleOrDefault<long>(
@"INSERT INTO [CustomerProfileSizeValues] (IsDeleted,[Value],FittingValue,MeasurementDefinitionId,CustomerProductProfileId)
VALUES (0,@Value,@FittingValue,@MeasurementDefinitionId,@CustomerProductProfileId)
SELECT SCOPE_IDENTITY();", sizeValue);

        public void UpdateSizeValue(CustomerProfileSizeValue sizeValue) =>
            _connection.Execute(
                "UPDATE [CustomerProfileSizeValues] SET [IsDeleted]=@IsDeleted, [Value]=@Value, [FittingValue]=@FittingValue WHERE [Id] = @Id",
                sizeValue);
    }
}
