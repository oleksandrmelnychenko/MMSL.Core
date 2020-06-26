using Dapper;
using MMSL.Domain.Entities.Measurements;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.Repositories.Stores.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Stores {
    class CustomerProductProfileRepository : ICustomerProductProfileRepository {

        private readonly IDbConnection _connection;

        private string _getByIdQuery =
@"SELECT [CustomerProductProfiles].*
,[CustomerProfileSizeValues].*
,[MeasurementDefinitions].*
FROM [CustomerProductProfiles]
LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [CustomerProductProfiles].DealerAccountId
LEFT JOIN [CustomerProfileSizeValues] ON [CustomerProfileSizeValues].CustomerProductProfileId = [CustomerProductProfiles].Id
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [CustomerProfileSizeValues].MeasurementDefinitionId
WHERE [CustomerProductProfiles].Id = @Id AND [CustomerProductProfiles].IsDeleted = 0";

        public CustomerProductProfileRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<CustomerProductProfile> GetCustomerProductProfilesByDealerIdentity(long dealerIdentityId, long? productCategoryId) {
            List<CustomerProductProfile> result = new List<CustomerProductProfile>();
            ;
            _connection.Query<CustomerProductProfile, CustomerProfileSizeValue, MeasurementDefinition, CustomerProductProfile>(
@"SELECT [CustomerProductProfiles].*
,[CustomerProfileSizeValues].*
,[MeasurementDefinitions].*
FROM [CustomerProductProfiles]
LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [CustomerProductProfiles].DealerAccountId
LEFT JOIN [CustomerProfileSizeValues] ON [CustomerProfileSizeValues].CustomerProductProfileId = [CustomerProductProfiles].Id
LEFT JOIN [MeasurementDefinitions] ON [MeasurementDefinitions].Id = [CustomerProfileSizeValues].MeasurementDefinitionId
WHERE [DealerAccount].UserIdentityId = @DealerIdentityId " +
(
    productCategoryId.HasValue && productCategoryId.Value != default(long) 
    ? "AND [CustomerProductProfiles].ProductCategoryId = @ProductCatagoryId " 
    : string.Empty
) +
"AND [CustomerProductProfiles].IsDeleted = 0",
                (profile, sizeValue, definition) => {
                    if (result.Any(x => x.Id == profile.Id)) {
                        profile = result.First(x => x.Id == profile.Id);
                    } else {
                        result.Add(profile);
                    }

                    if (sizeValue != null) {
                        sizeValue.MeasurementDefinition = definition;
                        profile.CustomerProfileSizeValues.Add(sizeValue);
                    }

                    return profile;
                },
                new {
                    DealerIdentityId = dealerIdentityId,
                    ProductCatagoryId = productCategoryId
                });

            return result;
        }

        public CustomerProductProfile GetCustomerProductProfile(long profileId) {
            CustomerProductProfile result = null;

            _connection.Query<CustomerProductProfile, CustomerProfileSizeValue, MeasurementDefinition, CustomerProductProfile>(
                _getByIdQuery,
                (profile, sizeValue, definition) => {
                    if (result == null) {
                        result = profile;
                    }

                    if (sizeValue != null) {
                        sizeValue.MeasurementDefinition = definition;
                        profile.CustomerProfileSizeValues.Add(sizeValue);
                    }

                    return profile;
                },
                new {
                    Id = profileId
                });

            return result;
        }

        public long AddCustomerProductProfile(CustomerProductProfile profile) =>
            _connection.QuerySingleOrDefault<long>(
@"INSERT INTO [CustomerProductProfiles] (IsDeleted,[Name],[Description],ProductCategoryId,StoreCustomerId,DealerAccountId,MeasurementId,FittingTypeId)
VALUES (0,@Name,@Description,@ProductCategoryId,@StoreCustomerId,@DealerAccountId,@MeasurementId,@FittingTypeId)
SELECT SCOPE_IDENTITY();",
                profile);

        public CustomerProductProfile UpdateCustomerProductProfile(CustomerProductProfile entity) {
            CustomerProductProfile result = null;

            _connection.Query<CustomerProductProfile, CustomerProfileSizeValue, MeasurementDefinition, CustomerProductProfile>(
                "UPDATE [CustomerProductProfiles] SET [Name]=@Name, [Description]=@Description, [IsDeleted]=@IsDeleted,"+
                "MeasurementId=@MeasurementId, FittingTypeId=@FittingTypeId "+
                "WHERE [CustomerProductProfiles].Id = @Id " +
                _getByIdQuery,
                (profile, sizeValue, definition) => {
                    if (result == null) {
                        result = profile;
                    }

                    if (sizeValue != null) {
                        sizeValue.MeasurementDefinition = definition;
                        profile.CustomerProfileSizeValues.Add(sizeValue);
                    }

                    return profile;
                },
                entity);

            return result;
        }
    }
}
