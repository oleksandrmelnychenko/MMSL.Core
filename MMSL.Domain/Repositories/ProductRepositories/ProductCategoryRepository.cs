using Dapper;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.DeliveryTimelines;
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
                "AND (SELECT COUNT([OptionGroups].Id) FROM [OptionGroups] WHERE [OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId AND [OptionGroups].IsDeleted = 0) > 0 " +
                "LEFT JOIN [OptionGroups] ON [OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId " +
                "AND [OptionGroups].IsDeleted = 0 " +
                "LEFT JOIN [OptionUnits] ON [OptionUnits].OptionGroupId = [OptionGroups].Id " +
                "AND [OptionUnits].IsDeleted = 0 " +
                "WHERE [ProductCategories].IsDeleted = 0" +
                "AND PATINDEX('%' + @SearchTerm + '%', [ProductCategories].Name) > 0 " +
                "ORDER BY [ProductCategories].Id, [OptionGroups].Id, [OptionUnits].OrderIndex",
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
                            productCategoryMapOptionGroup.OptionGroup = optionGroup;
                        }

                        if (optionUnit != null) {
                            productCategoryMapOptionGroup.OptionGroup.OptionUnits.Add(optionUnit);
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

        public ProductCategory GetDetailedById(long productCategoryId) {
            ProductCategory result = null;

            _connection.Query<ProductCategory, ProductCategoryMapOptionGroup, OptionGroup, OptionUnit, DeliveryTimelineProductMap, DeliveryTimeline, ProductCategory>(
                "SELECT [ProductCategories].*, [ProductCategoryMapOptionGroups].*, [OptionGroups].*, [OptionUnits].*, [DeliveryTimelineProductMaps].*, [DeliveryTimelines].*  " +
                "FROM [ProductCategories] " +

                "LEFT JOIN [ProductCategoryMapOptionGroups] ON [ProductCategoryMapOptionGroups].ProductCategoryId = [ProductCategories].Id " +
                "AND [ProductCategoryMapOptionGroups].IsDeleted = 0 " +
                "AND (SELECT COUNT([OptionGroups].Id) FROM [OptionGroups] WHERE [OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId AND [OptionGroups].IsDeleted = 0) > 0 " +
                "LEFT JOIN [OptionGroups] ON [OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId " +
                "AND [OptionGroups].IsDeleted = 0 " +
                "LEFT JOIN [OptionUnits] ON [OptionUnits].OptionGroupId = [OptionGroups].Id " +
                "AND [OptionUnits].IsDeleted = 0 " +
                
                "LEFT JOIN [DeliveryTimelineProductMaps] ON [DeliveryTimelineProductMaps].ProductCategoryId = [ProductCategories].Id " +
                "AND [DeliveryTimelineProductMaps].IsDeleted = 0 " +
                "LEFT JOIN [DeliveryTimelines] ON [DeliveryTimelines].Id = [DeliveryTimelineProductMaps].DeliveryTimelineId " +
                "AND [DeliveryTimelines].IsDeleted = 0 " +
                
                "WHERE [ProductCategories].Id = @Id AND [ProductCategories].IsDeleted = 0 " +
                "AND ( [ProductCategoryMapOptionGroups].Id IS NULL OR ([ProductCategoryMapOptionGroups].Id IS NOT NULL AND [OptionGroups].Id IS NOT NULL) ) " +
                "AND ( [DeliveryTimelineProductMaps].Id IS NULL OR ([DeliveryTimelineProductMaps].Id IS NOT NULL AND [DeliveryTimelines].Id IS NOT NULL) )" +
                "ORDER BY [ProductCategories].Id, [OptionGroups].Id, [OptionUnits].OrderIndex",
                (productCategory, productCategoryMapOptionGroup, optionGroup, optionUnit, deliveryTimelineProductMap, deliveryTimeline) => {
                    if (result == null) {
                        result = productCategory;
                    } else {
                        productCategory = result;
                    }

                    if (productCategoryMapOptionGroup != null) {
                        if (productCategory.OptionGroupMaps.Any(x => x.Id == productCategoryMapOptionGroup.Id)) {
                            productCategoryMapOptionGroup = productCategory.OptionGroupMaps.First(x => x.Id == productCategoryMapOptionGroup.Id);
                        } else {
                            productCategory.OptionGroupMaps.Add(productCategoryMapOptionGroup);
                            productCategoryMapOptionGroup.OptionGroup = optionGroup;
                        }

                        if (optionUnit != null) {
                            productCategoryMapOptionGroup.OptionGroup.OptionUnits.Add(optionUnit);
                        }
                    }
                   
                    if (deliveryTimelineProductMap != null) {
                        if (productCategory.DeliveryTimelineProductMaps.Any(x => x.Id == deliveryTimelineProductMap.Id)) {
                            deliveryTimelineProductMap = productCategory.DeliveryTimelineProductMaps.First(x => x.Id == deliveryTimelineProductMap.Id);
                        } else {
                            productCategory.DeliveryTimelineProductMaps.Add(deliveryTimelineProductMap);
                            deliveryTimelineProductMap.DeliveryTimeline = deliveryTimeline;
                        }
                    }

                    return productCategory;
                },
                new { Id = productCategoryId });

            return result;
        }

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
