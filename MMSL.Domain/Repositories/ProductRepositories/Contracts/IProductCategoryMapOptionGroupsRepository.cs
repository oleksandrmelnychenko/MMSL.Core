namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IProductCategoryMapOptionGroupsRepository {
        void NewMap(long productCategoryId, long optionGroupId);
    }
}
