﻿using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IProductCategoryRepository {
        List<ProductCategory> GetAll(string searchPhrase);

        ProductCategory NewProduct(ProductCategory newProductCategory);

        void UpdateProduct(ProductCategory product);
        ProductCategory GetById(long productCategoryId);
        ProductCategory GetDetailedById(long productCategoryId);
    }
}
