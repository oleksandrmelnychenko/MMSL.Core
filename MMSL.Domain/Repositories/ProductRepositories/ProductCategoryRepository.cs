using Dapper;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.ProductRepositories {
    public class ProductCategoryRepository : IProductCategoryRepository {

        private readonly IDbConnection _connection;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="connection"></param>
        public ProductCategoryRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<ProductCategory> GetAll(string searchPhrase) {
            List<ProductCategory> result = new List<ProductCategory>();

            _connection.Query<ProductCategory, ProductCategoryMapOptionGroup, OptionGroup, OptionUnit, ProductCategory>(
                "SELECT [ProductCategories].*, [ProductCategoryMapOptionGroups].*, [OptionGroups].*, [OptionUnits].* " +
                "FROM [ProductCategories] " +
                "LEFT JOIN [ProductCategoryMapOptionGroups] ON [ProductCategoryMapOptionGroups].ProductCategoryId = [ProductCategories].Id " +
                "AND [ProductCategoryMapOptionGroups].IsDeleted = 0 " +
                "LEFT JOIN [OptionGroups] ON [OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId " +
                "AND [OptionGroups].IsDeleted = 0 " +
                "LEFT JOIN [OptionUnits] ON [OptionUnits].OptionGroupId = [OptionGroups].Id " +
                "AND [OptionUnits].IsDeleted = 0 " +
                "WHERE [ProductCategories].IsDeleted = 0" +
                "AND PATINDEX('%' + @SearchTerm + '%', [ProductCategories].Name) > 0",
                (productCategory, productCategoryMapOptionGroup, optionGroup, optionUnit) => {
                    if (result.Any(x => x.Id == productCategory.Id)) {
                        productCategory = result.First(x => x.Id == productCategory.Id);
                    } else {
                        result.Add(productCategory);
                    }

                    if (productCategoryMapOptionGroup != null) {
                        if (productCategory.OptionGroupMaps.Any(x => x.Id == productCategoryMapOptionGroup.Id)) {
                            productCategoryMapOptionGroup = productCategory.OptionGroupMaps.First(x => x.Id == productCategoryMapOptionGroup.Id);
                        } else {
                            productCategory.OptionGroupMaps.Add(productCategoryMapOptionGroup);
                        }

                        if (optionGroup != null) {
                            productCategoryMapOptionGroup.OptionGroup = optionGroup;

                            if (optionUnit != null) {
                                optionGroup.OptionUnits.Add(optionUnit);
                            }
                        }
                    }

                    return productCategory;
                },
                new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase });

            return result;
        }

        public ProductCategory GetById(long productCategoryId) =>
            _connection.Query<ProductCategory>(
                "SELECT * " +
                "FROM [ProductCategories]" +
                "WHERE [ProductCategories].Id = @Id",
                new { Id = productCategoryId })
            .SingleOrDefault();

        public ProductCategory NewProduct(ProductCategory newProductCategory) =>
            _connection.Query<ProductCategory>(
                "INSERT INTO[ProductCategories](IsDeleted,[Name],[Description],[ImageUrl]) " +
                "VALUES(0,@Name,@Description,@ImageUrl) " +
                "SELECT[ProductCategories].* " +
                "FROM [ProductCategories] " +
                "WHERE [ProductCategories].Id = SCOPE_IDENTITY()",
                new {
                    Name = newProductCategory.Name,
                    Description = newProductCategory.Description,
                    ImageUrl = newProductCategory.ImageUrl
                })
            .SingleOrDefault();

        public void UpdateProduct(ProductCategory product) =>
            _connection.Execute(
                "UPDATE [ProductCategories] " +
                "SET [IsDeleted] = @IsDeleted,[LastModified]=getutcdate()," +
                "[Name]=@Name,[Description]=@Description,[ImageUrl] = @ImageUrl " +
                "WHERE [ProductCategories].Id = @Id", product);
    }
}
