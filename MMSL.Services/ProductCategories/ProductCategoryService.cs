using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Products;
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

        private readonly IDbConnectionFactory _connectionFactory;

        public ProductCategoryService(
            IProductCategoryRepositoriesFactory productCategoryRepositoriesFactory,
            IDbConnectionFactory connectionFactory) {

            _productCategoryRepositoriesFactory = productCategoryRepositoriesFactory;
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
    }
}
