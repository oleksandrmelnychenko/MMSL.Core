using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.ProductRepositories.Contracts {
    public interface IProductCategoryRepository {
        List<ProductCategory> GetAll(string searchPhrase, long? dealerAccountId);

        List<ProductCategory> GetAvailabilities(string searchPhrase, long? dealerAccountId);

        ProductCategory NewProduct(ProductCategory newProductCategory);

        void UpdateProduct(ProductCategory product);

        ProductCategory GetById(long productCategoryId);

        ProductCategory GetByIdForDealerIdentity(long productCategoryId, long dealerIdentityId, bool isBodyPostureOnly = false);

        ProductCategory GetDetailedById(long productCategoryId, bool isBodyPostureOnly = false);

        List<ProductCategory> GetAllByDealerIdentity(string searchPhrase, long? userIdentityId);
    }
}
