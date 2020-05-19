using Dapper;
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
    }
}
