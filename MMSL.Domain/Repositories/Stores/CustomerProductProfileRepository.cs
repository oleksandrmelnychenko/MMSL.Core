using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Entities.Options;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.Repositories.Stores.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Stores {
    class CustomerProductProfileRepository : ICustomerProductProfileRepository {

        private readonly IDbConnection _connection;

        private string _getByIdQuery =
@"SELECT [CustomerProductProfiles].*
,[ProductCategories].*
,[StoreCustomers].*
,[Measurements].*
,[MeasurementSizes].*
,[FittingTypes].*
,[CustomerProfileSizeValues].*
,[MeasurementDefinitions].*
,[CustomerProfileStyleConfigurations].*
,[OptionUnits].*
,[UnitValues].*
FROM [CustomerProductProfiles]
LEFT JOIN [ProductCategories] ON [ProductCategories].Id = [CustomerProductProfiles].ProductCategoryId 
LEFT JOIN [StoreCustomers] ON [StoreCustomers].Id = [CustomerProductProfiles].StoreCustomerId
LEFT JOIN [Measurements] ON [Measurements].Id = [CustomerProductProfiles].MeasurementId
LEFT JOIN [MeasurementSizes] ON [MeasurementSizes].Id = [CustomerProductProfiles].MeasurementSizeId
LEFT JOIN [FittingTypes] ON [FittingTypes].Id = [CustomerProductProfiles].FittingTypeId
LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [CustomerProductProfiles].DealerAccountId
LEFT JOIN [CustomerProfileSizeValues] ON [CustomerProfileSizeValues].CustomerProductProfileId = [CustomerProductProfiles].Id
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [CustomerProfileSizeValues].MeasurementDefinitionId

LEFT JOIN [CustomerProfileStyleConfigurations] ON [CustomerProfileStyleConfigurations].CustomerProductProfileId = [CustomerProductProfiles].Id 
AND [CustomerProfileStyleConfigurations].IsDeleted = 0
LEFT JOIN [OptionUnits] ON [OptionUnits].Id = [CustomerProfileStyleConfigurations].OptionUnitId
LEFT JOIN [UnitValues] ON [UnitValues].Id = [CustomerProfileStyleConfigurations].UnitValueId AND [UnitValues].OptionUnitId = [OptionUnits].Id

WHERE [CustomerProductProfiles].Id = @Id AND [CustomerProductProfiles].IsDeleted = 0";

        private Type[] _types = {
            typeof(CustomerProductProfile),
            typeof(ProductCategory),
            typeof(StoreCustomer),
            typeof(Measurement),
            typeof(MeasurementSize),
            typeof(FittingType),
            typeof(CustomerProfileSizeValue),
            typeof(MeasurementDefinition),

            typeof(CustomerProfileStyleConfiguration),
            typeof(OptionUnit),
            typeof(UnitValue)
        };

        public CustomerProductProfileRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<ProductCategory> GetProductsWithProfiles(long dealerIdentityId, long storeCustomerId) {
            List<ProductCategory> results = new List<ProductCategory>();

            string query =
@"SELECT [ProductCategories].*
,[CustomerProductProfiles].*
,[Measurements].*
,[MeasurementSizes].*
,[FittingTypes].*
,[CustomerProfileSizeValues].*
,[MeasurementDefinitions].*
,[CustomerProfileStyleConfigurations].*
,[OptionUnits].*
,[UnitValues].*
FROM [ProductCategories]
LEFT JOIN [ProductPermissionSettings] ON [ProductPermissionSettings].ProductCategoryId = [ProductCategories].Id AND [ProductPermissionSettings].IsDeleted = 0
LEFT JOIN [DealerMapProductPermissionSettings] ON [DealerMapProductPermissionSettings].ProductPermissionSettingsId = [ProductPermissionSettings].Id AND [DealerMapProductPermissionSettings].IsDeleted = 0
LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [DealerMapProductPermissionSettings].DealerAccountId AND [DealerAccount].UserIdentityId = @DealerIdentityId

LEFT JOIN [CustomerProductProfiles] ON [CustomerProductProfiles].ProductCategoryId = [ProductCategories].Id AND [CustomerProductProfiles].IsDeleted = 0 
AND [CustomerProductProfiles].StoreCustomerId = @StoreCustomerId
LEFT JOIN [Measurements] ON [Measurements].Id = [CustomerProductProfiles].MeasurementId
LEFT JOIN [MeasurementSizes] ON [MeasurementSizes].Id = [CustomerProductProfiles].MeasurementSizeId
LEFT JOIN [FittingTypes] ON [FittingTypes].Id = [CustomerProductProfiles].FittingTypeId
LEFT JOIN [CustomerProfileSizeValues] ON [CustomerProfileSizeValues].CustomerProductProfileId = [CustomerProductProfiles].Id
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [CustomerProfileSizeValues].MeasurementDefinitionId
LEFT JOIN [CustomerProfileStyleConfigurations] ON [CustomerProfileStyleConfigurations].CustomerProductProfileId = [CustomerProductProfiles].Id 
AND [CustomerProfileStyleConfigurations].IsDeleted = 0
LEFT JOIN [OptionUnits] ON [OptionUnits].Id = [CustomerProfileStyleConfigurations].OptionUnitId
LEFT JOIN [UnitValues] ON [UnitValues].Id = [CustomerProfileStyleConfigurations].UnitValueId AND [UnitValues].OptionUnitId = [OptionUnits].Id
WHERE [ProductCategories].IsDeleted = 0
AND [DealerAccount].Id IS NOT NULL";

            Type[] types = {
                typeof(ProductCategory),
                typeof(CustomerProductProfile),
                typeof(Measurement),
                typeof(MeasurementSize),
                typeof(FittingType),
                typeof(CustomerProfileSizeValue),
                typeof(MeasurementDefinition),
                typeof(CustomerProfileStyleConfiguration),
                typeof(OptionUnit),
                typeof(UnitValue)
            };

            Func<object[], ProductCategory> mapper = objects => {
                ProductCategory product = (ProductCategory)objects[0];
                CustomerProductProfile profile = (CustomerProductProfile)objects[1];
                Measurement measurement = (Measurement)objects[2];
                MeasurementSize size = (MeasurementSize)objects[3];
                FittingType fittingType = (FittingType)objects[4];
                CustomerProfileSizeValue sizeValue = (CustomerProfileSizeValue)objects[5];
                MeasurementDefinition definition = (MeasurementDefinition)objects[6];
                CustomerProfileStyleConfiguration styleConfiguration = (CustomerProfileStyleConfiguration)objects[7];
                OptionUnit unit = (OptionUnit)objects[8];
                UnitValue unitValue = (UnitValue)objects[9];

                if (results.Any(x => x.Id == product.Id)) {
                    product = results.First(x => x.Id == product.Id);
                } else {
                    results.Add(product);
                }

                if (product.CustomerProductProfiles.Any(x => x.Id == profile.Id)) {
                    profile = product.CustomerProductProfiles.First(x => x.Id == profile.Id);
                } else {
                    product.CustomerProductProfiles.Add(profile);
                }

                if (profile != null) {
                    profile.Measurement = measurement;
                    profile.MeasurementSize = size;
                    profile.FittingType = fittingType;

                    if (sizeValue != null) {
                        sizeValue.MeasurementDefinition = definition;
                        profile.CustomerProfileSizeValues.Add(sizeValue);
                    }

                    if (styleConfiguration != null) {
                        if (profile.CustomerProfileStyleConfigurations.Any(x => x.Id == styleConfiguration.Id)) {
                            styleConfiguration = profile.CustomerProfileStyleConfigurations.First(x => x.Id == styleConfiguration.Id);
                        } else {
                            profile.CustomerProfileStyleConfigurations.Add(styleConfiguration);
                        }

                        styleConfiguration.OptionUnit = unit;

                        if (unitValue != null) {
                            styleConfiguration.UnitValue = unitValue;
                        }
                    }

                }

                return null;
            };

            _connection.Query(query, types, mapper,
                new {
                    DealerIdentityId = dealerIdentityId,
                    StoreCustomerId = storeCustomerId
                });

            return results;
        }

        public List<CustomerProductProfile> GetCustomerProductProfilesByDealerIdentity(long dealerIdentityId, long? productCategoryId) {
            List<CustomerProductProfile> result = new List<CustomerProductProfile>();

            string query =
@"SELECT [CustomerProductProfiles].*
,[ProductCategories].*
,[StoreCustomers].*
,[Measurements].*
,[MeasurementSizes].*
,[FittingTypes].*
,[CustomerProfileSizeValues].*
,[MeasurementDefinitions].*
FROM [CustomerProductProfiles]
LEFT JOIN [ProductCategories] ON [ProductCategories].Id = [CustomerProductProfiles].ProductCategoryId 
LEFT JOIN [StoreCustomers] ON [StoreCustomers].Id = [CustomerProductProfiles].StoreCustomerId
LEFT JOIN [Measurements] ON [Measurements].Id = [CustomerProductProfiles].MeasurementId
LEFT JOIN [MeasurementSizes] ON [MeasurementSizes].Id = [CustomerProductProfiles].MeasurementSizeId
LEFT JOIN [FittingTypes] ON [FittingTypes].Id = [CustomerProductProfiles].FittingTypeId
LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [CustomerProductProfiles].DealerAccountId
LEFT JOIN [CustomerProfileSizeValues] ON [CustomerProfileSizeValues].CustomerProductProfileId = [CustomerProductProfiles].Id
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [CustomerProfileSizeValues].MeasurementDefinitionId
WHERE [DealerAccount].UserIdentityId = @DealerIdentityId  " +
(
    productCategoryId.HasValue && productCategoryId.Value != default(long)
    ? "AND [CustomerProductProfiles].ProductCategoryId = @ProductCatagoryId "
    : string.Empty
) +
"AND [CustomerProductProfiles].IsDeleted = 0";

            Type[] types = {
                    typeof(CustomerProductProfile),
                    typeof(ProductCategory),
                    typeof(StoreCustomer),
                    typeof(Measurement),
                    typeof(MeasurementSize),
                    typeof(FittingType),
                    typeof(CustomerProfileSizeValue),
                    typeof(MeasurementDefinition)
                };

            Func<object[], CustomerProductProfile> mapper = objects => {
                CustomerProductProfile profile = (CustomerProductProfile)objects[0];
                ProductCategory product = (ProductCategory)objects[1];
                StoreCustomer customer = (StoreCustomer)objects[2];
                Measurement measurement = (Measurement)objects[3];
                MeasurementSize size = (MeasurementSize)objects[4];
                FittingType fitting = (FittingType)objects[5];
                CustomerProfileSizeValue sizeValue = (CustomerProfileSizeValue)objects[6];
                MeasurementDefinition definition = (MeasurementDefinition)objects[7];

                if (result.Any(x => x.Id == profile.Id)) {
                    profile = result.First(x => x.Id == profile.Id);
                } else {
                    result.Add(profile);
                }

                profile.ProductCategory = product;
                profile.StoreCustomer = customer;
                profile.Measurement = measurement;
                profile.MeasurementSize = size;
                profile.FittingType = fitting;

                if (sizeValue != null) {
                    sizeValue.MeasurementDefinition = definition;
                    profile.CustomerProfileSizeValues.Add(sizeValue);
                }

                return profile;
            };

            _connection.Query(query, types, mapper,
                new {
                    DealerIdentityId = dealerIdentityId,
                    ProductCatagoryId = productCategoryId
                });

            return result;
        }

        public CustomerProductProfile GetCustomerProductProfile(long profileId) {
            CustomerProductProfile result = null;

            Func<object[], CustomerProductProfile> mapper = objects => {
                CustomerProductProfile profile = (CustomerProductProfile)objects[0];
                ProductCategory product = (ProductCategory)objects[1];
                StoreCustomer customer = (StoreCustomer)objects[2];
                Measurement measurement = (Measurement)objects[3];
                MeasurementSize size = (MeasurementSize)objects[4];
                FittingType fitting = (FittingType)objects[5];
                CustomerProfileSizeValue sizeValue = (CustomerProfileSizeValue)objects[6];
                MeasurementDefinition definition = (MeasurementDefinition)objects[7];

                CustomerProfileStyleConfiguration styleConfig = (CustomerProfileStyleConfiguration)objects[8];
                OptionUnit unit = (OptionUnit)objects[9];
                UnitValue unitValue = (UnitValue)objects[10];

                if (result == null) {
                    result = profile;
                }

                result.ProductCategory = product;
                result.StoreCustomer = customer;
                result.Measurement = measurement;
                result.MeasurementSize = size;
                result.FittingType = fitting;

                if (sizeValue != null && result.CustomerProfileSizeValues.All(x => x.Id != sizeValue.Id)) {
                    sizeValue.MeasurementDefinition = definition;
                    result.CustomerProfileSizeValues.Add(sizeValue);
                }

                if (styleConfig != null) {
                    if (result.CustomerProfileStyleConfigurations.Any(x => x.Id == styleConfig.Id)) {
                        styleConfig = result.CustomerProfileStyleConfigurations.First(x => x.Id == styleConfig.Id);
                    } else {
                        result.CustomerProfileStyleConfigurations.Add(styleConfig);
                    }

                    styleConfig.OptionUnit = unit;

                    if (unitValue != null) {
                        styleConfig.UnitValue = unitValue;
                    }
                }

                return profile;
            };

            _connection.Query(_getByIdQuery, _types, mapper,
                new {
                    Id = profileId
                });

            return result;
        }

        public long AddCustomerProductProfile(CustomerProductProfile profile) =>
            _connection.QuerySingleOrDefault<long>(
@"INSERT INTO [CustomerProductProfiles] (IsDeleted,[Name],[Description],ProductCategoryId,StoreCustomerId,DealerAccountId,MeasurementId,FittingTypeId,MeasurementSizeId,ProfileType)
VALUES (0,@Name,@Description,@ProductCategoryId,@StoreCustomerId,@DealerAccountId,@MeasurementId,@FittingTypeId,@MeasurementSizeId,@ProfileType)
SELECT SCOPE_IDENTITY();",
                profile);

        public CustomerProductProfile UpdateCustomerProductProfile(CustomerProductProfile entity) {
            CustomerProductProfile result = null;

            _connection.Query(
                "UPDATE [CustomerProductProfiles] SET [Name]=@Name, [Description]=@Description, [IsDeleted]=@IsDeleted," +
                "MeasurementId=@MeasurementId, FittingTypeId=@FittingTypeId, MeasurementSizeId=@MeasurementSizeId,ProfileType=@ProfileType,[StoreCustomerId]=@StoreCustomerId " +
                "WHERE [CustomerProductProfiles].Id = @Id " +
                _getByIdQuery, _types,
                objects => {
                    CustomerProductProfile profile = (CustomerProductProfile)objects[0];
                    ProductCategory product = (ProductCategory)objects[1];
                    StoreCustomer customer = (StoreCustomer)objects[2];
                    Measurement measurement = (Measurement)objects[3];
                    MeasurementSize size = (MeasurementSize)objects[4];
                    FittingType fitting = (FittingType)objects[5];
                    CustomerProfileSizeValue sizeValue = (CustomerProfileSizeValue)objects[6];
                    MeasurementDefinition definition = (MeasurementDefinition)objects[7];

                    CustomerProfileStyleConfiguration styleConfig = (CustomerProfileStyleConfiguration)objects[8];
                    OptionUnit unit = (OptionUnit)objects[9];
                    UnitValue unitValue = (UnitValue)objects[10];

                    if (result == null) {
                        result = profile;
                    }

                    result.ProductCategory = product;
                    result.StoreCustomer = customer;
                    result.Measurement = measurement;
                    result.MeasurementSize = size;
                    result.FittingType = fitting;

                    if (sizeValue != null) {
                        sizeValue.MeasurementDefinition = definition;
                        result.CustomerProfileSizeValues.Add(sizeValue);
                    }

                    if (styleConfig != null) {
                        if (result.CustomerProfileStyleConfigurations.Any(x => x.Id == styleConfig.Id)) {
                            styleConfig = result.CustomerProfileStyleConfigurations.First(x => x.Id == styleConfig.Id);
                        } else {
                            result.CustomerProfileStyleConfigurations.Add(styleConfig);
                        }

                        styleConfig.OptionUnit = unit;

                        if (unitValue != null) {
                            styleConfig.UnitValue = unitValue;
                        }
                    }

                    return profile;
                },
                entity);

            return result;
        }
    }
}
