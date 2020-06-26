using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.DeliveryTimelines.Contracts;
using MMSL.Domain.Repositories.Options.Contracts;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using MMSL.Services.ProductCategories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories {
    public class ProductCategoryService : IProductCategoryService {

        private readonly IProductCategoryRepositoriesFactory _productCategoryRepositoriesFactory;

        private readonly IOptionRepositoriesFactory _optionRepositoriesFactory;

        private readonly IDeliveryTimelineRepositoriesFactory _deliveryTimelineRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="productCategoryRepositoriesFactory"></param>
        /// <param name="connectionFactory"></param>
        /// <param name="optionGroupRepository"></param>
        public ProductCategoryService(
            IProductCategoryRepositoriesFactory productCategoryRepositoriesFactory,
            IDbConnectionFactory connectionFactory,
            IOptionRepositoriesFactory optionRepositoriesFactory,
            IDeliveryTimelineRepositoriesFactory deliveryTimelineRepositoriesFactory) {

            _productCategoryRepositoriesFactory = productCategoryRepositoriesFactory;
            _optionRepositoriesFactory = optionRepositoriesFactory;
            _connectionFactory = connectionFactory;
            _deliveryTimelineRepositoriesFactory = deliveryTimelineRepositoriesFactory;
        }

        public Task<List<ProductCategory>> GetProductCategoriesAsync(string searchPhrase, long? dealerAccountId, long? userIdentityId, bool isForDealer = false) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductCategoryRepository repository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);

                    return isForDealer
                        ? repository.GetAllByDealerIdentity(searchPhrase, userIdentityId)
                        : repository.GetAll(searchPhrase, dealerAccountId);
                }
            });

        public Task<ProductCategory> GetProductCategoryAsync(long productCategoryId, bool includeDetails = false) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductCategoryRepository repo = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);

                    return includeDetails ? repo.GetDetailedById(productCategoryId) : repo.GetById(productCategoryId);
                }
            });

        public Task<ProductCategory> NewProductCategoryAsync(ProductCategory newProductCategory, IEnumerable<long> groupIds = null) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductCategoryRepository productCategoryRepository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);
                    IProductCategoryMapOptionGroupsRepository productCategoryMapOptionGroupsRepository = _productCategoryRepositoriesFactory.NewProductCategoryMapOptionGroupsRepository(connection);

                    ProductCategory product = null;

                    product = productCategoryRepository.NewProduct(newProductCategory);

                    if (groupIds != null && groupIds.Any() && product != null) {
                        foreach (long optionGroupId in groupIds) {
                            productCategoryMapOptionGroupsRepository.NewMap(product.Id, optionGroupId);
                        }
                    }

                    return product;
                }
            });

        public Task UpdateProductCategoryAsync(ProductCategory product) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductCategoryRepository productCategoryRepository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);

                    productCategoryRepository.UpdateProduct(product);
                }
            });

        public Task DeleteProductCategoryAsync(long productCategoryId) =>
             Task.Run(() => {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                     IProductCategoryRepository productCategoryRepository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);

                     ProductCategory product = productCategoryRepository.GetById(productCategoryId);

                     if (product == null) throw new Exception("ProductCategory not found");

                     product.IsDeleted = true;

                     productCategoryRepository.UpdateProduct(product);
                 }
             });

        public Task UpdateProductCategoryOptionGroupsAsync(IEnumerable<ProductCategoryMapOptionGroup> maps) =>
              Task.Run(() => {
                  using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                      IProductCategoryRepository productCategoryRepository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);
                      IProductCategoryMapOptionGroupsRepository productCategoryMapOptionGroupsRepository = _productCategoryRepositoriesFactory.NewProductCategoryMapOptionGroupsRepository(connection);
                      IOptionGroupRepository optionGroupRepository = _optionRepositoriesFactory.NewOptionGroupRepository(connection);

                      foreach (ProductCategoryMapOptionGroup map in maps) {
                          if (map.Id != default(long) && map.IsDeleted) {
                              ProductCategoryMapOptionGroup toDelete = productCategoryMapOptionGroupsRepository.GetById(map.Id);
                              if (toDelete != null) {
                                  toDelete.IsDeleted = true;
                                  productCategoryMapOptionGroupsRepository.UpdateMap(toDelete);
                              }
                          } else {
                              productCategoryMapOptionGroupsRepository.NewMap(map.ProductCategoryId, map.OptionGroupId);
                          }
                      }
                  }
              });

        public Task<List<ProductCategory>> GetProductCategoriesAvailabilitiesAsync(string searchPhrase, long? dealerAccountId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    List<ProductCategory> products = null;
                    IProductCategoryRepository productCategoryRepository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);
                    products = productCategoryRepository.GetAvailabilities(searchPhrase, dealerAccountId);
                    return products;
                }
            });
    }
}
