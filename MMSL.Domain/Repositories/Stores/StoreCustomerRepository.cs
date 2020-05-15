using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.StoreCustomers;
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
                "LEFT JOIN [Address] AS [Billing] ON [Billing].Id = [StoreCustomers].[BillingAddressId] " +
                "LEFT JOIN [Address] AS [Delivery] ON [Delivery].Id = [StoreCustomers].[DeliveryAddressId] " +
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

        public PaginatingResult<StoreCustomer> GetStoreCustomers(long? storeId, int pageNumber, int limit, string searchPhrase) {
            PaginatingResult<StoreCustomer> result = new PaginatingResult<StoreCustomer>();

            PagerIdParams pager = new PagerIdParams(storeId.HasValue ? storeId.Value : 0, pageNumber, limit, searchPhrase);

            string query = ";WITH [Paginated_StoreCustomer_CTE] AS ( " +
                "SELECT [StoreCustomers].Id, ROW_NUMBER() OVER(ORDER BY [StoreCustomers].UserName) AS [RowNumber] " +
                "FROM [StoreCustomers] " +
                "WHERE [StoreCustomers].IsDeleted = 0";

            if (storeId.HasValue) {
                query += "AND [StoreId] = @Id ";
            }

            if (!string.IsNullOrEmpty(searchPhrase)) {
                query += "AND (" +
                    "PATINDEX('%' + @SearchTerm + '%', [StoreCustomers].UserName) > 0 " +
                    "OR PATINDEX('%' + @SearchTerm + '%', [StoreCustomers].CustomerName) > 0 " +
                    ")";
            }

            query += ") " +
                "SELECT [Paginated_StoreCustomer_CTE].RowNumber, [StoreCustomers].* " +
                "FROM [StoreCustomers] " +
                "LEFT JOIN [Paginated_StoreCustomer_CTE] ON [Paginated_StoreCustomer_CTE].Id = [StoreCustomers].Id " +
                "WHERE [StoreCustomers].IsDeleted = 0 " +
                "AND [Paginated_StoreCustomer_CTE].RowNumber > @Offset " +
                "AND [Paginated_StoreCustomer_CTE].RowNumber <= @Offset + @Limit " +
                "ORDER BY [Paginated_StoreCustomer_CTE].RowNumber";

            result.Entities = _connection.Query<StoreCustomer>(query, pager)
                .ToList();

            string paginatingDetailQuery = "SELECT TOP(1) " +
                "[PageSize]= @Limit," +
                "[PageNumber] = (@Offset / @Limit) + 1, " +
                "[TotalItems] = COUNT(DISTINCT [StoreCustomers].Id), " +
                "[PagesCount] = CEILING(CONVERT(float, COUNT(DISTINCT[StoreCustomers].Id)) / @Limit) " +
                "FROM [StoreCustomers] " +
                "WHERE [StoreCustomers].[IsDeleted] = 0 ";

            if (storeId.HasValue) {
                query += "AND [StoreId] = @Id ";
            }

            paginatingDetailQuery +=
                "AND (" +
                "PATINDEX('%' + @SearchTerm + '%', [StoreCustomers].UserName) > 0 " +
                "OR PATINDEX('%' + @SearchTerm + '%', [StoreCustomers].CustomerName) > 0 " +
                ")";

            result.PaginationInfo = _connection.QuerySingle<PaginationInfo>(paginatingDetailQuery, pager);

            return result;
        }

        public void UpdateStoreCustomer(StoreCustomer storeCustomer) =>
            _connection.Query<StoreCustomer>("UPDATE [StoreCustomers] " +
                "SET [IsDeleted]=@IsDeleted,[UserName]=@UserName,[CustomerName]=@CustomerName," +
                "[Email]=@Email,[PhoneNumber]=@PhoneNumber,[BirthDate]=@BirthDate," +
                "[UseBillingAsDeliveryAddress]=@UseBillingAsDeliveryAddress," +
                "[BillingAddressId]=@BillingAddressId,[DeliveryAddressId]=@DeliveryAddressId," +
                "[StoreId]=@StoreId " +
                "WHERE [StoreCustomers].Id=@Id;", storeCustomer);
    }
}
