using Dapper;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.ProductRepositories {
    class DealerProductAvailabilityRepository : IDealerProductAvailabilityRepository {

        private readonly IDbConnection _connection;

        public DealerProductAvailabilityRepository(IDbConnection connection) {
            _connection = connection;
        }

        public long AddAvailability(DealerProductAvailability dealerProductAvailability) =>
            _connection.QuerySingleOrDefault<long>(
@"INSERT INTO [DealerProductAvailabilities] ([IsDeleted],[DealerAccountId],[ProductCategoryId],[IsDisabled]) 
VALUES (0,@DealerAccountId,@ProductCategoryId,@IsDisabled) 
SELECT SCOPE_IDENTITY()",
                dealerProductAvailability);

        public void DeleteAvailability(DealerProductAvailability dealerProductAvailability) =>
            _connection.Execute(
@"DELETE [DealerProductAvailabilities] 
WHERE [DealerProductAvailabilities].DealerAccountId = @DealerAccountId 
AND [DealerProductAvailabilities].ProductCategoryId = @ProductCategoryId",
                dealerProductAvailability);

        public void UpdateAvailability(DealerProductAvailability dealerProductAvailability) =>
            _connection.Execute(
@"UPDATE [DealerProductAvailabilities] 
SET [DealerAccountId] = @DealerAccountId,
[ProductCategoryId] = @ProductCategoryId,
[IsDisabled] = @IsDisabled 
[IsDeleted] = @IsDeleted
WHERE [DealerProductAvailabilities].Id = @Id",
                dealerProductAvailability);
    }
}
