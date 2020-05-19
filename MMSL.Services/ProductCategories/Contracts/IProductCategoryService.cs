﻿using MMSL.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.ProductCategories.Contracts {
    public interface IProductCategoryService {
        Task<List<ProductCategory>> GetProductCategoriesAsync(string searchPhrase);
    }
}
