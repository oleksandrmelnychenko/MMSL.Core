using Dapper;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.ProductRepositories {
    class ProductPermissionSettingsRepository : IProductPermissionSettingsRepository {

        private readonly IDbConnection _connection;

        public ProductPermissionSettingsRepository(IDbConnection connection) {
            _connection = connection;
        }

        public ProductPermissionSettings AddProductPermissionSettings(ProductPermissionSettings productSettings) {
            long id = _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [ProductPermissionSettings] ([IsDeleted], [Name], [Description], [ProductCategoryId]) " +
                "VALUES (0, @Name, @Description, @ProductCategoryId);" +
                "SELECT SCOPE_IDENTITY()",
                productSettings);

            return GetProductPermissionSettingsById(id);
        }

        public List<ProductPermissionSettings> GetProductPermissionSettingsByProduct(long productId, string dealerSearchTerm) {
            List<ProductPermissionSettings> result = new List<ProductPermissionSettings>();

            string query = 
                "SELECT [ProductPermissionSettings].*, " +
                "(SELECT COUNT([DealerMapProductPermissionSettings].Id) " +
                "FROM [DealerMapProductPermissionSettings] " +
                "LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [DealerMapProductPermissionSettings].DealerAccountId " +
                "AND [DealerAccount].IsDeleted = 0 " +
                "WHERE [DealerMapProductPermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id " +
                "AND [DealerMapProductPermissionSettings].IsDeleted = 0 " +
                "AND [DealerAccount].Id IS NOT NULL) AS [DealersAppliedCount]" +
                ", [PermissionSettings].* " +
                "FROM [ProductPermissionSettings] " +
                "LEFT JOIN [PermissionSettings] ON [PermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id " +
                "AND [PermissionSettings].IsDeleted = 0 " +
                "WHERE [ProductPermissionSettings].ProductCategoryId = @ProductId " +
                "AND [ProductPermissionSettings].IsDeleted = 0";
            //"AND PATINDEX('%' + @SearchTerm + '%', [ProductCategories].Name) > 0"

            _connection.Query<ProductPermissionSettings, PermissionSettings, ProductPermissionSettings>(
                query,
                (productPermissionSettings, permissionSettings) => {
                    if (result.Any(x => x.Id == productPermissionSettings.Id)) {
                        productPermissionSettings = result.First(x => x.Id == productPermissionSettings.Id);
                    } else {
                        result.Add(productPermissionSettings);
                    }

                    if (permissionSettings != null) {
                        productPermissionSettings.PermissionSettings.Add(permissionSettings);
                    }

                    return productPermissionSettings;
                },
                new { ProductId = productId });

            return result;
        }

        public ProductPermissionSettings GetProductPermissionSettingsById(long productSettingsId, bool includeDeletedSettings = false) {
            ProductPermissionSettings result = null;

            _connection.Query<ProductPermissionSettings, PermissionSettings, ProductPermissionSettings>(
                "SELECT [ProductPermissionSettings].*" +
                ", (SELECT COUNT(Id) " +
                "FROM [DealerMapProductPermissionSettings] " +
                "WHERE [DealerMapProductPermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id " +
                "AND [DealerMapProductPermissionSettings].IsDeleted = 0) AS [DealersAppliedCount]" +
                ", [PermissionSettings].* " +
                "FROM [ProductPermissionSettings] " +
                "LEFT JOIN [PermissionSettings] ON [PermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id " +
                (includeDeletedSettings ? string.Empty : "AND [PermissionSettings].IsDeleted = 0 " ) +
                "WHERE [ProductPermissionSettings].Id = @Id " +
                "AND [ProductPermissionSettings].IsDeleted = 0",
                (productPermissionSettings, permissionSettings) => {
                    if (result == null) {
                        result = productPermissionSettings;
                    }

                    if (permissionSettings != null) {
                        result.PermissionSettings.Add(permissionSettings);
                    }

                    return productPermissionSettings;
                },
                new { Id = productSettingsId });

            return result;
        }

        public ProductPermissionSettings UpdateProductPermissionSettings(ProductPermissionSettings productSettings) {
            _connection.Execute(
                "UPDATE [ProductPermissionSettings] " +
                "SET [IsDeleted] = @IsDeleted, [LastModified] = GETUTCDATE(), [Name] = @Name, [Description] = @Description " +
                "WHERE [ProductPermissionSettings].Id = @Id",
                productSettings);

            return GetProductPermissionSettingsById(productSettings.Id);
        }
    }
}
