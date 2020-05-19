using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Data;

namespace MMSL.Domain.Repositories.ProductRepositories {
    public class ProductCategoryRepositoriesFactory : IProductCategoryRepositoriesFactory {
        public IProductCategoryRepository NewProductCategoryRepository(IDbConnection connection) =>
            new ProductCategoryRepository(connection);

        public IProductCategoryMapOptionGroupsRepository NewProductCategoryMapOptionGroupsRepository(IDbConnection connection) =>
         new ProductCategoryMapOptionGroupsRepository(connection);
    }
}
