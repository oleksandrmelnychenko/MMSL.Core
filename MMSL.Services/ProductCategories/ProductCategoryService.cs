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

        public Task<ProductCategory> NewProductCategoryAsync(NewProductCategoryDataContract newProductCategoryDataContract) =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    IProductCategoryRepository productCategoryRepository = _productCategoryRepositoriesFactory.NewProductCategoryRepository(connection);
                    var productCategoryMapOptionGroupsRepository = _productCategoryRepositoriesFactory.NewProductCategoryMapOptionGroupsRepository(connection);

                    ProductCategory product = null;

                    product = productCategoryRepository.NewProduct(newProductCategoryDataContract);

                    if (newProductCategoryDataContract.OptionGroupIds != null && newProductCategoryDataContract.OptionGroupIds.Any() && product != null) {
                        foreach (long optionGroupId in newProductCategoryDataContract.OptionGroupIds) {
                            productCategoryMapOptionGroupsRepository.NewMap(product.Id, optionGroupId);
                        }
                    }

                    return product;
                }
            });
    }
}
