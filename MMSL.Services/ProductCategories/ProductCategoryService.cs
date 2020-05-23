using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Products;
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
            IOptionRepositoriesFactory optionRepositoriesFactory) {

            _productCategoryRepositoriesFactory = productCategoryRepositoriesFactory;
            _optionRepositoriesFactory = optionRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<List<ProductCategory>> GetProductCategoriesAsync(string searchPhrase) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    List<ProductCategory> products = null;
                    IProductCategoryRepository productCategoryRepository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);
                    products = productCategoryRepository.GetAll(searchPhrase);
                    return products;
                }
            });

        public Task<ProductCategory> GetProductCategoryAsync(long productCategoryId) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    return _productCategoryRepositoriesFactory
                        .NewProductCategoryRepository(connection)
                        .GetById(productCategoryId);
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
    }
}
