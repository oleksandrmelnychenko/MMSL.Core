using System.Data;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IProductCategoryRepositoriesFactory {
        IProductCategoryRepository NewProductCategoryRepository(IDbConnection connection);

        IProductCategoryMapOptionGroupsRepository NewProductCategoryMapOptionGroupsRepository(IDbConnection connection);
    }
}
