using Dapper;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.ProductRepositories {
    public class ProductCategoryMapOptionGroupsRepository : IProductCategoryMapOptionGroupsRepository {

        private readonly IDbConnection _connection;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connection"></param>
        public ProductCategoryMapOptionGroupsRepository(IDbConnection connection) {
            _connection = connection;
        }

        public void NewMap(long productCategoryId, long optionGroupId) =>
            _connection.Query<long>(
                "INSERT INTO [ProductCategoryMapOptionGroups] (IsDeleted,[ProductCategoryId],[OptionGroupId])" +
                "VALUES(0,@ProductCategoryId,@OptionGroupId); " +
                "SELECT SCOPE_IDENTITY()",
                new { ProductCategoryId = productCategoryId, OptionGroupId = optionGroupId });
    }
}
