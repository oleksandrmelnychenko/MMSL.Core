using Dapper;
using MMSL.Domain.DataContracts;
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

        public List<ProductCategory> GetAll(string searchPhrase) =>
             _connection.Query<ProductCategory>(
                "SELECT *" +
                "FROM [ProductCategories] " +
                "WHERE IsDeleted = 0 AND PATINDEX('%' + @SearchTerm + '%', [ProductCategories].Name) > 0",
                new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase })
            .ToList();

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
