using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Entities.Products;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Dealer.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Dealer {
    public class DealerAccountRepository : IDealerAccountRepository {

        private readonly IDbConnection _connection;

        public DealerAccountRepository(IDbConnection connection) {
            _connection = connection;
        }

        public long AddDealerAccount(DealerAccount dealerAccount) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [DealerAccount] " +
                "([IsDeleted],[CompanyName],[Name],[Email],[AlternateEmail],[PhoneNumber]," +
                "[TaxNumber],[IsVatApplicable],[CurrencyTypeId],[PaymentTypeId],[IsCreditAllowed],[BillingAddressId]," +
                "[UseBillingAsShipping],[ShippingAddressId]) " +
                "VALUES (0,@CompanyName,@Name,@Email,@AlternateEmail,@PhoneNumber," +
                "@TaxNumber,@IsVatApplicable,@CurrencyTypeId,@PaymentTypeId,@IsCreditAllowed,@BillingAddressId," +
                "@UseBillingAsShipping,@ShippingAddressId); " +
                "SELECT SCOPE_IDENTITY()", dealerAccount);

        public PaginatingResult<DealerAccount> GetDealerAccounts(int pageNumber, int limit, string searchPhrase, DateTime? from, DateTime? to) {

            PaginatingResult<DealerAccount> result = new PaginatingResult<DealerAccount>();

            PagerParams pager = new PagerParams(pageNumber, limit, searchPhrase);

            string paginatingDetailQuery = "SELECT TOP(1) " +
                "[PageSize]= @Limit," +
                "[PageNumber] = (@Offset / @Limit) + 1, " +
                "[TotalItems] = COUNT(DISTINCT [DealerAccount].Id), " +
                "[PagesCount] = CEILING(CONVERT(float, COUNT(DISTINCT[DealerAccount].Id)) / @Limit) " +
                "FROM [DealerAccount] " +
                "WHERE [DealerAccount].[IsDeleted] = 0 ";

            string query = ";WITH [Paginated_Dealers_CTE] AS ( " +
                "SELECT [DealerAccount].Id, ROW_NUMBER() OVER(ORDER BY [DealerAccount].CompanyName) AS [RowNumber] " +
                "FROM [DealerAccount] " +
                "WHERE [DealerAccount].IsDeleted = 0 ";

            if (!string.IsNullOrEmpty(searchPhrase)) {
                string searchPart = "AND (" +
                    "PATINDEX('%' + @SearchTerm + '%', [DealerAccount].CompanyName) > 0 " +
                    "OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].Email) > 0 " +
                    "OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].PhoneNumber) > 0 " +
                    ")";

                paginatingDetailQuery += searchPart;
                query += searchPart;

            }

            if (from.HasValue) {
                string dateFromFilterPart = "AND [Created] >= Convert(datetime, @From)";

                paginatingDetailQuery += dateFromFilterPart;
                query += dateFromFilterPart;
            }

            if (to.HasValue) {
                string dateToFilterPart = "AND [Created] <= Convert(datetime, @To)";

                paginatingDetailQuery += dateToFilterPart;
                query += dateToFilterPart;
            }

            query += ") SELECT [Paginated_Dealers_CTE].RowNumber, [DealerAccount].* " +
                "FROM [DealerAccount] " +
                "LEFT JOIN [Paginated_Dealers_CTE] ON [Paginated_Dealers_CTE].Id = [DealerAccount].Id " +
                "WHERE [DealerAccount].IsDeleted = 0 " +
                "AND [Paginated_Dealers_CTE].RowNumber > @Offset " +
                "AND [Paginated_Dealers_CTE].RowNumber <= @Offset + @Limit " +
                "ORDER BY [Paginated_Dealers_CTE].RowNumber";

            result.Entities = _connection.Query<DealerAccount>(query,
                new {
                    pager.Offset,
                    pager.Limit,
                    pager.SearchTerm,
                    From = from,
                    To = to
                })
                .ToList();

            result.PaginationInfo = _connection.QuerySingle<PaginationInfo>(paginatingDetailQuery,
                new {
                    pager.Offset,
                    pager.Limit,
                    pager.SearchTerm,
                    From = from,
                    To = to
                });

            return result;
        }

        public DealerAccount GetDealerAccount(long dealerAccountId) =>
            _connection.Query<DealerAccount, Address, Address, DealerAccount>(
                "SELECT [DealerAccount].*, Billing.*, Shipping.* " +
                "FROM [DealerAccount] " +
                "LEFT JOIN [Address] AS Billing " +
                "ON Billing.Id = [DealerAccount].BillingAddressId AND [DealerAccount].BillingAddressId IS NOT NULL " +
                "LEFT JOIN [Address] AS Shipping " +
                "ON Shipping.Id = [DealerAccount].BillingAddressId AND [DealerAccount].ShippingAddressId IS NOT NULL " +
                "WHERE [DealerAccount].Id = @Id",
                (dealerAccount, billingAddress, shippingAddress) => {
                    dealerAccount.BillingAddress = billingAddress;
                    dealerAccount.ShippingAddress = shippingAddress;

                    return dealerAccount;
                },
                new { Id = dealerAccountId })
            .SingleOrDefault();

        public void UpdateDealerAccount(DealerAccount dealerAccount) =>
            _connection.Query<DealerAccount>("UPDATE [DealerAccount]" +
                "SET [IsDeleted]=@IsDeleted,[LastModified]=getutcdate()," +
                "[Name]=@Name," +
                "[CompanyName]=@CompanyName," +
                "[Email]=@Email," +
                "[AlternateEmail]=@AlternateEmail," +
                "[PhoneNumber]=@PhoneNumber," +
                "[TaxNumber]=@TaxNumber," +
                "[IsVatApplicable]=@IsVatApplicable," +
                "[CurrencyTypeId]=@CurrencyTypeId," +
                "[PaymentTypeId]=@PaymentTypeId," +
                "[IsCreditAllowed]=@IsCreditAllowed," +
                "[BillingAddressId]=@BillingAddressId," +
                "[UseBillingAsShipping]=@UseBillingAsShipping," +
                "[ShippingAddressId]=@ShippingAddressId " +
                "WHERE [DealerAccount].Id=@Id;",
                dealerAccount);

        public List<DealerAccount> GetDealerAccounts(long productPermissionSettingId) =>
            _connection.Query<DealerAccount>(
                "SELECT [DealerAccount].* " +
                "FROM [DealerAccount] " +
                "LEFT JOIN [DealerMapProductPermissionSettings] ON [DealerMapProductPermissionSettings].DealerAccountId = [DealerAccount].Id " +
                "WHERE [DealerAccount].IsDeleted = 0 " +
                "AND [DealerMapProductPermissionSettings].IsDeleted = 0" +
                "AND DealerMapProductPermissionSettings.ProductPermissionSettingsId = @ProductPermissionSettingId",
                new {
                    ProductPermissionSettingId = productPermissionSettingId
                })
                .ToList();

        public List<DealerAccount> SearchDealerAccounts(string searchPhrase, long? productId = null, bool? excludeMatchProduct = null) {
            List<DealerAccount> dealerAccounts = new List<DealerAccount>();

            string query =
                "SELECT [DealerAccount].*" +
                ", [DealerMapProductPermissionSettings].*" +
                ", [ProductPermissionSettings].* " +
                "FROM [DealerAccount] " +
                "LEFT JOIN [DealerMapProductPermissionSettings] ON [DealerMapProductPermissionSettings].DealerAccountId = [DealerAccount].Id " +
                "AND [DealerMapProductPermissionSettings].IsDeleted = 0 " +
                "LEFT JOIN [ProductPermissionSettings] ON [ProductPermissionSettings].Id = [DealerMapProductPermissionSettings].ProductPermissionSettingsId " +
                "AND [ProductPermissionSettings].IsDeleted = 0 " +
                "AND [ProductPermissionSettings].Id IS NOT NULL " +
                "AND [ProductPermissionSettings].ProductCategoryId = @ProductId " +
                "WHERE [DealerAccount].IsDeleted = 0 " +
                (
                excludeMatchProduct.HasValue && excludeMatchProduct.Value 
                    ? "AND [ProductPermissionSettings].Id IS NULL " 
                    : "AND ([DealerMapProductPermissionSettings].Id IS NULL " +
                    "OR ([DealerMapProductPermissionSettings].Id IS NOT NULL AND [ProductPermissionSettings].Id IS NOT NULL))"
                ) +
                "AND (" +
                "PATINDEX('%' + @SearchPhrase + '%', [DealerAccount].CompanyName) > 0 " +
                "OR PATINDEX('%' + @SearchPhrase + '%', [DealerAccount].Email) > 0 " +
                "OR PATINDEX('%' + @SearchPhrase + '%', [DealerAccount].Name) > 0 " +
                ")";

            _connection.Query<DealerAccount, DealerMapProductPermissionSettings, ProductPermissionSettings, DealerAccount>(
                query,
                (dealerAccount, dealerMapProductPermissionSettings, productPermissionSettings) => {
                    if (dealerAccounts.Any(x => x.Id == dealerAccount.Id)) {
                        dealerAccount = dealerAccounts.First(x => x.Id == dealerAccount.Id);
                    } else {
                        dealerAccounts.Add(dealerAccount);
                    }

                    if (dealerMapProductPermissionSettings != null && productPermissionSettings != null) {
                        dealerMapProductPermissionSettings.ProductPermissionSettings = productPermissionSettings;
                        dealerAccount.DealerMapProductPermissionSettings.Add(dealerMapProductPermissionSettings);
                    }

                    return dealerAccount;
                },
                new {
                    SearchPhrase = searchPhrase,
                    ProductId = productId
                })
                .ToList();

            return dealerAccounts;
        }
    }
}
