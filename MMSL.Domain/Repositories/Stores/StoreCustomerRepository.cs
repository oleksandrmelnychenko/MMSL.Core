using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Domain.Entities.Stores;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Stores.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Stores {
    public class StoreCustomerRepository : IStoreCustomerRepository {

        private readonly IDbConnection _connection;

        /// <summary>
        /// ctor().
        /// </summary>
        /// <param name="connection"></param>
        public StoreCustomerRepository(IDbConnection connection) {
            _connection = connection;
        }

        public long AddStoreCustomer(StoreCustomer storeCustomer) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [StoreCustomers] " +
                "([IsDeleted],[UserName],[CustomerName],[Email],[PhoneNumber],[BirthDate]," +
                "[UseBillingAsDeliveryAddress],[BillingAddressId],[DeliveryAddressId],[StoreId]) " +
                "VALUES (0,@UserName,@CustomerName,@Email,@PhoneNumber,@BirthDate," +
                "@UseBillingAsDeliveryAddress,@BillingAddressId,@DeliveryAddressId,@StoreId);" +
                "SELECT SCOPE_IDENTITY()", storeCustomer);

        public StoreCustomer GetStoreCustomer(long storeCustomerId) =>
            _connection.Query<StoreCustomer, Address, Address, StoreCustomer>(
                "SELECT [StoreCustomers].*, [Billing].*, [Delivery].* " +
                "FROM [StoreCustomers] " +
                "LEFT JOIN [Address] AS [Billing] ON [Billing].Id = [StoreCustomers].[BillingAddressId] AND [Billing].IsDeleted = 0 " +
                "LEFT JOIN [Address] AS [Delivery] ON [Delivery].Id = [StoreCustomers].[DeliveryAddressId] AND [Delivery].IsDeleted = 0 " +
                "WHERE [StoreCustomers].Id = @Id AND [StoreCustomers].[IsDeleted] = 0",
                (dealerAccount, billingAddress, deliveryAddress) => {
                    dealerAccount.BillingAddress = billingAddress;
                    dealerAccount.DeliveryAddress = deliveryAddress;

                    return dealerAccount;
                },
                new {
                    Id = storeCustomerId
                })
            .SingleOrDefault();

        public PaginatingResult<StoreCustomer> GetStoreCustomers(int pageNumber, int limit, string searchPhrase, string storeName, long? storeId = null, long? userIdentityId = null) {
            PaginatingResult<StoreCustomer> result = new PaginatingResult<StoreCustomer>();

            PagerIdParams pagerModel = new PagerIdParams(storeId.HasValue ? storeId.Value : 0, pageNumber, limit, searchPhrase);

            var queryParams = new {
                pagerModel.Id,
                pagerModel.Limit,
                pagerModel.Offset,
                pagerModel.SearchTerm,
                StoreName = storeName,
                DealerIdentityId = userIdentityId
            };

            string paginatingDetailQuery = "SELECT TOP(1) " +
               "[PageSize]= @Limit," +
               "[PageNumber] = (@Offset / @Limit) + 1, " +
               "[TotalItems] = COUNT(DISTINCT [StoreCustomers].Id), " +
               "[PagesCount] = CEILING(CONVERT(float, COUNT(DISTINCT[StoreCustomers].Id)) / @Limit) " +
               "FROM [StoreCustomers] " +
               "LEFT JOIN [Stores] ON [Stores].Id = [StoreCustomers].StoreId AND [Stores].IsDeleted = 0 " +
               "LEFT JOIN [StoreMapDealerAccounts] ON [StoreMapDealerAccounts].StoreId = [Stores].Id AND [StoreMapDealerAccounts].IsDeleted = 0 " +
               "LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [StoreMapDealerAccounts].DealerAccountId AND [DealerAccount].IsDeleted = 0 " +
               "WHERE [StoreCustomers].[IsDeleted] = 0 " +
               "AND [Stores].Id IS NOT NULL " +
               "AND [DealerAccount].Id IS NOT NULL " + 
               (
                    userIdentityId.HasValue ?
                    "AND [DealerAccount].UserIdentityId = @DealerIdentityId "
                    : String.Empty
               );

            string query = ";WITH [Paginated_StoreCustomer_CTE] AS ( " +
                "SELECT [StoreCustomers].Id, ROW_NUMBER() OVER(ORDER BY [StoreCustomers].UserName) AS [RowNumber] " +
                "FROM [StoreCustomers] " +
                "LEFT JOIN [Stores] ON [Stores].Id = [StoreCustomers].StoreId AND [Stores].IsDeleted = 0 " +
                "LEFT JOIN [StoreMapDealerAccounts] ON [StoreMapDealerAccounts].StoreId = [Stores].Id AND [StoreMapDealerAccounts].IsDeleted = 0 " +
                "LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [StoreMapDealerAccounts].DealerAccountId AND [DealerAccount].IsDeleted = 0 " +
                "WHERE [StoreCustomers].IsDeleted = 0 " +
                "AND [Stores].Id IS NOT NULL " +
                "AND [DealerAccount].Id IS NOT NULL " +
                (
                    userIdentityId.HasValue ?
                    "AND [DealerAccount].UserIdentityId = @DealerIdentityId "
                    : String.Empty
                );

            if (storeId.HasValue) {
                string storeIdQueryPart = "AND [StoreId] = @Id ";

                paginatingDetailQuery += storeIdQueryPart;
                query += storeIdQueryPart;
            } else if (!string.IsNullOrEmpty(storeName)) {
                string searchByStorePart = "AND PATINDEX('%' + @StoreName + '%', [Stores].[Name]) > 0 ";

                paginatingDetailQuery += searchByStorePart;
                query += searchByStorePart;
            }

            if (!string.IsNullOrEmpty(searchPhrase)) {
                string searchQueryPart = "AND (" +
                    "PATINDEX('%' + @SearchTerm + '%', [StoreCustomers].UserName) > 0 " +
                    "OR PATINDEX('%' + @SearchTerm + '%', [StoreCustomers].CustomerName) > 0 " +
                    ")";

                paginatingDetailQuery += searchQueryPart;
                query += searchQueryPart;
            }

            query += ") " +
                "SELECT [Paginated_StoreCustomer_CTE].RowNumber, [StoreCustomers].*, [Stores].* " +
                "FROM [StoreCustomers] " +
                "LEFT JOIN [Paginated_StoreCustomer_CTE] ON [Paginated_StoreCustomer_CTE].Id = [StoreCustomers].Id " +
                "LEFT JOIN [Stores] ON [Stores].Id = [StoreCustomers].StoreId AND [Stores].IsDeleted = 0 " +
                "LEFT JOIN [StoreMapDealerAccounts] ON [StoreMapDealerAccounts].StoreId = [Stores].Id AND [StoreMapDealerAccounts].IsDeleted = 0 " +
                "LEFT JOIN [DealerAccount] ON [DealerAccount].Id = [StoreMapDealerAccounts].DealerAccountId AND [DealerAccount].IsDeleted = 0 " +
                "WHERE [StoreCustomers].IsDeleted = 0 " +
                "AND [Paginated_StoreCustomer_CTE].RowNumber > @Offset " +
                "AND [Paginated_StoreCustomer_CTE].RowNumber <= @Offset + @Limit " +
                "AND [Stores].Id IS NOT NULL " +
                "AND [DealerAccount].Id IS NOT NULL " +
                (
                    userIdentityId.HasValue ? 
                    "AND [DealerAccount].UserIdentityId = @DealerIdentityId "
                    : String.Empty
                ) +
                "ORDER BY [Paginated_StoreCustomer_CTE].RowNumber";

            result.Entities = _connection.Query<StoreCustomer, Store, StoreCustomer>(
                query,
                (storeCustomer, store) => {
                    storeCustomer.Store = store;
                    return storeCustomer;
                },
                queryParams).ToList();

            result.PaginationInfo = _connection.QuerySingle<PaginationInfo>(paginatingDetailQuery, queryParams);

            return result;
        }

        public void UpdateStoreCustomer(StoreCustomer storeCustomer) =>
            _connection.Query<StoreCustomer>("UPDATE [StoreCustomers] " +
                "SET [IsDeleted]=@IsDeleted,[UserName]=@UserName,[CustomerName]=@CustomerName," +
                "[Email]=@Email,[PhoneNumber]=@PhoneNumber,[BirthDate]=@BirthDate," +
                "[UseBillingAsDeliveryAddress]=@UseBillingAsDeliveryAddress," +
                "[BillingAddressId]=@BillingAddressId,[DeliveryAddressId]=@DeliveryAddressId," +
                "[StoreId]=@StoreId, [LastModified] = GETUTCDATE() " +
                "WHERE [StoreCustomers].Id=@Id;", storeCustomer);
    }
}
