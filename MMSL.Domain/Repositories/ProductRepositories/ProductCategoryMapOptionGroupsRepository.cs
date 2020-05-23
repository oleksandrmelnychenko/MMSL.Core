using Dapper;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Data;
using System.Linq;

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

        public ProductCategoryMapOptionGroup GetById(long id) =>
            _connection.Query<ProductCategoryMapOptionGroup>(
                  "SELECT * " +
                  "FROM[ProductCategoryMapOptionGroups] " +
                  "WHERE [IsDeleted] = 0 AND [ProductCategoryMapOptionGroups].Id = @Id",
                  new { Id = id })
              .SingleOrDefault();

            public ProductCategoryMapOptionGroup GetByIds(long productCategoryId, long optionGroupId) =>
            _connection.Query<ProductCategoryMapOptionGroup>(
                "SELECT * " +
                "FROM[ProductCategoryMapOptionGroups] " +
                "WHERE [IsDeleted] = 0 AND [ProductCategoryId] = @ProductCategoryId AND [OptionGroupId] = @OptionGroupId",
                new { ProductCategoryId = productCategoryId, OptionGroupId = optionGroupId })
            .SingleOrDefault();

        public void NewMap(long productCategoryId, long optionGroupId) =>
            _connection.Query<long>(
                "INSERT INTO [ProductCategoryMapOptionGroups] (IsDeleted,[ProductCategoryId],[OptionGroupId])" +
                "VALUES(0,@ProductCategoryId,@OptionGroupId); " +
                "SELECT SCOPE_IDENTITY()",
                new { ProductCategoryId = productCategoryId, OptionGroupId = optionGroupId });

        public void UpdateMap(ProductCategoryMapOptionGroup map) =>
            _connection.Execute(
                "UPDATE [ProductCategoryMapOptionGroups] " +
                "SET [IsDeleted] = @IsDeleted,[LastModified]=getutcdate() " +
                "WHERE [ProductCategoryMapOptionGroups].Id = @Id", map);
    }
}
