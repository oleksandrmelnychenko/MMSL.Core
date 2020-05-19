using MMSL.Domain.Entities.Products;
using MMSL.Services.ProductCategories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories {
    public class ProductCategoryService : IProductCategoryService {

        public Task<List<ProductCategory>> GetProductCategoriesAsync(string searchPhrase) {
            throw new NotImplementedException();
        }
    }
}
