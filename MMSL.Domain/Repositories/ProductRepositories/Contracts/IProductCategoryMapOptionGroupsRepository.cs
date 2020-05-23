using MMSL.Domain.Entities.Products;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IProductCategoryMapOptionGroupsRepository {

        void NewMap(long productCategoryId, long optionGroupId);

        void UpdateMap(ProductCategoryMapOptionGroup map);

        ProductCategoryMapOptionGroup GetByIds(long productCategoryId, long optionGroupId);

        ProductCategoryMapOptionGroup GetById(long id);
    }
}
