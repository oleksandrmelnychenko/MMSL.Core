using Dapper;
using MMSL.Domain.Entities.Measurements;
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
FROM [CustomerProductProfiles]
LEFT JOIN [ProductCategories] ON [ProductCategories].Id = [CustomerProductProfiles].ProductCategoryId 
LEFT JOIN [StoreCustomers] ON [StoreCustomers].Id = [CustomerProductProfiles].StoreCustomerId
LEFT JOIN [Measurements] ON [Measurements].Id = [CustomerProductProfiles].MeasurementId
LEFT JOIN [MeasurementSizes] ON [MeasurementSizes].Id = [CustomerProductProfiles].MeasurementSizeId
LEFT JOIN [FittingTypes] ON [FittingTypes].Id = [CustomerProductProfiles].FittingTypeId
LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [CustomerProductProfiles].DealerAccountId
LEFT JOIN [CustomerProfileSizeValues] ON [CustomerProfileSizeValues].CustomerProductProfileId = [CustomerProductProfiles].Id
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [CustomerProfileSizeValues].MeasurementDefinitionId
WHERE [CustomerProductProfiles].Id = @Id AND [CustomerProductProfiles].IsDeleted = 0";

        private Type[] _types = {
                    typeof(CustomerProductProfile),
                    typeof(ProductCategory),
                    typeof(StoreCustomer),
                    typeof(Measurement),
                    typeof(MeasurementSize),
                    typeof(FittingType),
                    typeof(CustomerProfileSizeValue),
                    typeof(MeasurementDefinition)
                };

        public CustomerProductProfileRepository(IDbConnection connection) {
            _connection = connection;
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
                "MeasurementId=@MeasurementId, FittingTypeId=@FittingTypeId, MeasurementSizeId=@MeasurementSizeId,ProfileType=@ProfileType " +
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

                    return profile;
                },
                entity);

            return result;
        }
    }
}
