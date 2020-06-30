using Dapper;
using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.CurrencyTypes;
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

        public List<ProductCategory> GetAll(string searchPhrase, long? dealerAccountId) {
            List<ProductCategory> result = new List<ProductCategory>();

            string query =
                "SELECT [ProductCategories].*, [ProductCategoryMapOptionGroups].*, [OptionGroups].*, [OptionUnits].* " +
                "FROM [ProductCategories] " +
                "LEFT JOIN [ProductCategoryMapOptionGroups] ON [ProductCategoryMapOptionGroups].ProductCategoryId = [ProductCategories].Id " +
                "AND [ProductCategoryMapOptionGroups].IsDeleted = 0 " +
                "AND (SELECT COUNT([OptionGroups].Id) FROM [OptionGroups] WHERE [OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId AND [OptionGroups].IsDeleted = 0) > 0 " +
                "LEFT JOIN [OptionGroups] ON [OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId " +
                "AND [OptionGroups].IsDeleted = 0 " +
                "LEFT JOIN [OptionUnits] ON [OptionUnits].OptionGroupId = [OptionGroups].Id " +
                "AND [OptionUnits].IsDeleted = 0 " +
                (
                    dealerAccountId.HasValue && dealerAccountId.Value != default(long)
                    ? "LEFT JOIN [DealerProductAvailabilities] ON [DealerProductAvailabilities].ProductCategoryId = ProductCategories.Id "
                    : string.Empty
                ) +
                "WHERE [ProductCategories].IsDeleted = 0 " +
                "AND PATINDEX('%' + @SearchTerm + '%', [ProductCategories].Name) > 0 " +
                (
                    dealerAccountId.HasValue && dealerAccountId.Value != default(long)
                    ? "AND ([DealerProductAvailabilities].Id IS NULL OR [DealerProductAvailabilities].DealerAccountId != @DealerAccountId) "
                    : string.Empty
                ) +
                "ORDER BY [ProductCategories].Id, [OptionGroups].Id, [OptionUnits].OrderIndex";

            _connection.Query<ProductCategory, ProductCategoryMapOptionGroup, OptionGroup, OptionUnit, ProductCategory>(
                query,
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
                new {
                    SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase,
                    DealerAccountId = dealerAccountId
                });

            return result;
        }

        public List<ProductCategory> GetAllByDealerIdentity(string searchPhrase, long? userIdentityId) {
            List<ProductCategory> result = new List<ProductCategory>();

            string query =
@"SELECT [ProductCategories].*
,[ProductCategoryMapOptionGroups].*
,[OptionGroups].*
,[OptionUnits].*
FROM[ProductCategories]
LEFT JOIN[ProductPermissionSettings] ON[ProductPermissionSettings].ProductCategoryId = [ProductCategories].Id
AND[ProductPermissionSettings].IsDeleted = 0
LEFT JOIN[PermissionSettings] ON[PermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id
AND[PermissionSettings].IsDeleted = 0
AND[PermissionSettings].IsAllow = 1
LEFT JOIN[ProductCategoryMapOptionGroups] ON[ProductCategoryMapOptionGroups].ProductCategoryId = [ProductCategories].Id
AND[ProductCategoryMapOptionGroups].IsDeleted = 0
AND[ProductCategoryMapOptionGroups].OptionGroupId = [PermissionSettings].OptionGroupId
AND(SELECT COUNT([OptionGroups].Id) FROM[OptionGroups] WHERE[OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId AND[OptionGroups].IsDeleted = 0) > 0
LEFT JOIN[OptionGroups] ON[OptionGroups].Id = [PermissionSettings].OptionGroupId
AND[OptionGroups].IsDeleted = 0
LEFT JOIN[OptionUnits] ON[OptionUnits].OptionGroupId = [OptionGroups].Id
AND[OptionUnits].Id = [PermissionSettings].OptionUnitId
AND[OptionUnits].IsDeleted = 0
LEFT JOIN[DealerMapProductPermissionSettings] ON[DealerMapProductPermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id AND[DealerMapProductPermissionSettings].IsDeleted = 0
LEFT JOIN[DealerAccount] ON[DealerAccount].Id = [DealerMapProductPermissionSettings].DealerAccountId
WHERE[ProductCategories].IsDeleted = 0
AND[DealerMapProductPermissionSettings].Id IS NOT NULL
AND[DealerAccount].Id IS NOT NULL
AND[DealerAccount].UserIdentityId = @UserIdentityId
AND PATINDEX('%' + @SearchTerm + '%', [ProductCategories].Name) > 0";

            _connection.Query<ProductCategory, ProductCategoryMapOptionGroup, OptionGroup, OptionUnit, ProductCategory>(
                query,
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
                new {
                    SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase,
                    UserIdentityId = userIdentityId
                });


            return result;
        }

        public List<ProductCategory> GetAvailabilities(string searchPhrase, long? dealerAccountId) =>
            _connection.Query<ProductCategory>(
@"SELECT [ProductCategories].*,[DealerProductAvailabilities].IsDisabled 
FROM [ProductCategories]
LEFT JOIN [DealerProductAvailabilities] ON [DealerProductAvailabilities].ProductCategoryId = [ProductCategories].Id 
AND DealerAccountId = @DealerAccountId 
WHERE [ProductCategories].IsDeleted = 0 ",
                new { DealerAccountId = dealerAccountId }
                ).ToList();

        public ProductCategory GetByIdForDealerIdentity(long productCategoryId, long dealerIdentityId) {
            ProductCategory result = null;

            string query =
@"SELECT [ProductCategories].*
,[ProductCategoryMapOptionGroups].*
,[OptionGroups].*
,[GroupPrice].*
,[GroupCurrency].*
,[OptionUnits].*
,[UnitValues].*
,[UnitPrice].*
,[UnitCurrency].*
,[DeliveryTimelineProductMaps].*
,[DeliveryTimelines].* 
FROM[ProductCategories]

LEFT JOIN [ProductPermissionSettings] ON[ProductPermissionSettings].ProductCategoryId = [ProductCategories].Id
AND [ProductPermissionSettings].IsDeleted = 0
LEFT JOIN[PermissionSettings] ON[PermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id
AND [PermissionSettings].IsDeleted = 0
AND [PermissionSettings].IsAllow = 1
LEFT JOIN[ProductCategoryMapOptionGroups] ON[ProductCategoryMapOptionGroups].ProductCategoryId = [ProductCategories].Id
AND [ProductCategoryMapOptionGroups].IsDeleted = 0
AND [ProductCategoryMapOptionGroups].OptionGroupId = [PermissionSettings].OptionGroupId
AND(SELECT COUNT([OptionGroups].Id) FROM[OptionGroups] WHERE[OptionGroups].Id = [ProductCategoryMapOptionGroups].OptionGroupId AND[OptionGroups].IsDeleted = 0) > 0
LEFT JOIN [OptionGroups] ON [OptionGroups].Id = [PermissionSettings].OptionGroupId
AND [OptionGroups].IsDeleted = 0
LEFT JOIN [OptionPrices] AS [GroupPrice] ON [GroupPrice].OptionGroupId = OptionGroups.Id AND [GroupPrice].IsDeleted = 0
LEFT JOIN [CurrencyTypes] AS [GroupCurrency] ON [GroupCurrency].Id = [GroupPrice].CurrencyTypeId
LEFT JOIN [OptionUnits] ON[OptionUnits].OptionGroupId = [OptionGroups].Id
AND [OptionUnits].Id = [PermissionSettings].OptionUnitId
AND [OptionUnits].IsDeleted = 0
LEFT JOIN [UnitValues] ON [UnitValues].OptionUnitId = [OptionUnits].Id AND [UnitValues].IsDeleted = 0
LEFT JOIN [OptionPrices] AS [UnitPrice] ON [UnitPrice].OptionUnitId = [OptionUnits].Id AND [UnitPrice].IsDeleted = 0 
LEFT JOIN [CurrencyTypes] AS [UnitCurrency] ON [UnitCurrency].Id = [UnitPrice].CurrencyTypeId
LEFT JOIN [DealerMapProductPermissionSettings] ON[DealerMapProductPermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id AND[DealerMapProductPermissionSettings].IsDeleted = 0
LEFT JOIN [DealerAccount] ON[DealerAccount].Id = [DealerMapProductPermissionSettings].DealerAccountId
LEFT JOIN [DeliveryTimelineProductMaps] ON [DeliveryTimelineProductMaps].ProductCategoryId = [ProductCategories].Id  
AND [DeliveryTimelineProductMaps].IsDeleted = 0  
AND (SELECT COUNT([DeliveryTimelines].Id) FROM [DeliveryTimelines] WHERE [DeliveryTimelines].Id = [DeliveryTimelineProductMaps].DeliveryTimelineId AND [DeliveryTimelines].IsDeleted = 0) > 0  
LEFT JOIN [DeliveryTimelines] ON [DeliveryTimelines].Id = [DeliveryTimelineProductMaps].DeliveryTimelineId  
AND [DeliveryTimelines].IsDeleted = 0  

WHERE[ProductCategories].IsDeleted = 0
AND[DealerMapProductPermissionSettings].Id IS NOT NULL
AND[DealerAccount].Id IS NOT NULL
AND[DealerAccount].UserIdentityId = @UserIdentityId
AND[ProductCategories].Id=@ProductCategoryId";

            Type[] types = new Type[] {
                typeof(ProductCategory),
                typeof(ProductCategoryMapOptionGroup),
                typeof(OptionGroup),
                typeof(OptionPrice),
                typeof(CurrencyType),
                typeof(OptionUnit),
                typeof(UnitValue),
                typeof(OptionPrice),
                typeof(CurrencyType),
                typeof(DeliveryTimelineProductMap),
                typeof(DeliveryTimeline)
            };

            Func<object[], ProductCategory> mapper = objects => {
                ProductCategory productCategory = (ProductCategory)objects[0];//
                ProductCategoryMapOptionGroup productCategoryMapOptionGroup = (ProductCategoryMapOptionGroup)objects[1];//
                OptionGroup optionGroup = (OptionGroup)objects[2];//
                OptionPrice groupPrice = (OptionPrice)objects[3];
                CurrencyType groupCurrency = (CurrencyType)objects[4];
                OptionUnit optionUnit = (OptionUnit)objects[5];
                UnitValue unitValue = (UnitValue)objects[6];
                OptionPrice unitPrice = (OptionPrice)objects[7];
                CurrencyType unitCurrency = (CurrencyType)objects[8];
                DeliveryTimelineProductMap deliveryTimelineProductMap = (DeliveryTimelineProductMap)objects[9];
                DeliveryTimeline deliveryTimeline = (DeliveryTimeline)objects[10];

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

                        if (groupPrice != null) {
                            groupPrice.CurrencyType = groupCurrency;
                            productCategoryMapOptionGroup.OptionGroup.CurrentPrice = groupPrice;
                        }
                    }

                    if (optionUnit != null) {

                        if (productCategoryMapOptionGroup.OptionGroup.OptionUnits.Any(x => x.Id == optionUnit.Id)) {
                            optionUnit = productCategoryMapOptionGroup.OptionGroup.OptionUnits.First(x => x.Id == optionUnit.Id);
                        } else {
                            productCategoryMapOptionGroup.OptionGroup.OptionUnits.Add(optionUnit);
                        }

                        if (unitValue != null) {
                            optionUnit.UnitValues.Add(unitValue);
                        }

                        if (unitPrice != null) {
                            unitPrice.CurrencyType = unitCurrency;
                            optionUnit.CurrentPrice = unitPrice;
                        }

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
            };

            _connection.Query(query, types, mapper,
                new {
                    ProductCategoryId = productCategoryId,
                    UserIdentityId = dealerIdentityId
                });

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
                "AND (SELECT COUNT([DeliveryTimelines].Id) FROM [DeliveryTimelines] WHERE [DeliveryTimelines].Id = [DeliveryTimelineProductMaps].DeliveryTimelineId AND [DeliveryTimelines].IsDeleted = 0) > 0 " +
                "LEFT JOIN [DeliveryTimelines] ON [DeliveryTimelines].Id = [DeliveryTimelineProductMaps].DeliveryTimelineId " +
                "AND [DeliveryTimelines].IsDeleted = 0 " +

                "WHERE [ProductCategories].Id = @Id AND [ProductCategories].IsDeleted = 0 " +
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
