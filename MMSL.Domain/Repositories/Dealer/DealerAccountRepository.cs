﻿using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Dealer.Contracts;
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
                "([IsDeleted],[CompanyName],[Email],[AlternateEmail],[PhoneNumber]," +
                "[TaxNumber],[IsVatApplicable],[Currency],[PaymentType],[IsCreditAllowed],[BillingAddressId]," +
                "[UseBillingAsShipping],[ShippingAddressId]) " +
                "VALUES (0,@CompanyName,@Email,@AlternateEmail,@PhoneNumber," +
                "@TaxNumber,@IsVatApplicable,@Currency,@PaymentType,@IsCreditAllowed,@BillingAddressId," +
                "@UseBillingAsShipping,@ShippingAddressId); " +
                "SELECT SCOPE_IDENTITY()", dealerAccount);

        public PaginatingResult<DealerAccount> GetDealerAccounts(int pageNumber, int limit, string searchPhrase) {

            PaginatingResult<DealerAccount> result = new PaginatingResult<DealerAccount>();

            PagerParams pager = new PagerParams(pageNumber, limit, searchPhrase);

            string query = ";WITH [Paginated_Dealers_CTE] AS ( " +
                "SELECT [DealerAccount].Id, ROW_NUMBER() OVER(ORDER BY [DealerAccount].CompanyName) AS [RowNumber] " +
                "FROM [DealerAccount] " +
                "WHERE [DealerAccount].IsDeleted = 0 ";

            if (!string.IsNullOrEmpty(searchPhrase)) {
                query += "AND (" +
                    "PATINDEX('%' + @SearchTerm + '%', [DealerAccount].CompanyName) > 0 " +
                    "OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].Email) > 0 " +
                    "OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].PhoneNumber) > 0 " +
                    ")";
            }

            query += ") SELECT [Paginated_Dealers_CTE].RowNumber, [DealerAccount].* " +
                "FROM [DealerAccount] " +
                "LEFT JOIN [Paginated_Dealers_CTE] ON [Paginated_Dealers_CTE].Id = [DealerAccount].Id " +
                "WHERE [DealerAccount].IsDeleted = 0 " +
                "AND [Paginated_Dealers_CTE].RowNumber > @Offset " +
                "AND [Paginated_Dealers_CTE].RowNumber <= @Offset + @Limit " +
                "ORDER BY [Paginated_Dealers_CTE].RowNumber";

            result.Entities = _connection.Query<DealerAccount>(query, pager)
                .ToList();

            string paginatingDetailQuery = "SELECT TOP(1) " +
                "[PageSize]= @Limit," +
                "[PageNumber] = (@Offset / @Limit) + 1, " +
                "[TotalItems] = COUNT(DISTINCT [DealerAccount].Id), " +
                "[PagesCount] = CEILING(CONVERT(float, COUNT(DISTINCT[DealerAccount].Id)) / @Limit) " +
                "FROM [DealerAccount] " +
                "WHERE [DealerAccount].[IsDeleted] = 0 " +
                "AND (" +
                "PATINDEX('%' + @SearchTerm + '%', [DealerAccount].CompanyName) > 0 " +
                "OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].Email) > 0 " +
                "OR PATINDEX('%' + @SearchTerm + '%', [DealerAccount].PhoneNumber) > 0 " +
                ")";

            result.PaginationInfo = _connection.QuerySingle<PaginationInfo>(paginatingDetailQuery, pager);

            return result;
        }

        public DealerAccount GetDealerAccount(long dealerAccountId) =>
            _connection.Query<DealerAccount, Address, Address, DealerAccount>(
                "SELECT [DealerAccount].*, Billing.*, Shipping.* " +
                "FROM[DealerAccount] " +
                "LEFT JOIN[Address] AS Billing " +
                "ON Billing.Id = [DealerAccount].BillingAddressId AND [DealerAccount].BillingAddressId IS NOT NULL " +
                "LEFT JOIN[Address] AS Shipping " +
                "ON Shipping.Id = [DealerAccount].BillingAddressId AND [DealerAccount].ShippingAddressId IS NOT NULL " +
                "WHERE[DealerAccount].Id = @Id",
                (dealerAccount, billingAddress, shippingAddress) => {
                    dealerAccount.BillingAddress = billingAddress;
                    dealerAccount.ShippingAddress = shippingAddress;

                    return dealerAccount;
                },
                new { Id = dealerAccountId })
            .SingleOrDefault();

        public void UpdateDealerAccount(DealerAccount dealerAccount) =>
            _connection.Query<DealerAccount>("UPDATE [DealerAccount]" +
                "SET [IsDeleted]=@IsDeleted,[Created]=@Created,[LastModified]=getutcdate()," +
                "[CompanyName]=@CompanyName,[Email]=@Email," +
                "[AlternateEmail]=@AlternateEmail,[PhoneNumber]=@PhoneNumber,[TaxNumber]=@TaxNumber," +
                "[IsVatApplicable]=@IsVatApplicable,[Currency]=@Currency,[PaymentType]=@PaymentType," +
                "[IsCreditAllowed]=@IsCreditAllowed," +
                "[BillingAddressId]=@BillingAddressId," +
                "[UseBillingAsShipping]=@UseBillingAsShipping," +
                "[ShippingAddressId]=@ShippingAddressId " +
                "WHERE [DealerAccount].Id=@Id;",
                dealerAccount);
    }
}
