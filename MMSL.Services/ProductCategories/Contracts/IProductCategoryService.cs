﻿using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories.Contracts {
    public interface IProductCategoryService {
        Task<List<ProductCategory>> GetProductCategoriesAsync(string searchPhrase);
        Task<ProductCategory> NewProductCategoryAsync(ProductCategory newProductCategory, IEnumerable<long> groupIds = null);
        Task UpdateProductCategoryAsync(ProductCategory product);
        Task DeleteProductCategoryAsync(long productCategoryId);
    }
}