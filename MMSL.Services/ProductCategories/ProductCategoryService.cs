using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using MMSL.Services.ProductCategories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
