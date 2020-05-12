using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
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
                "([IsDeleted],[Name],[Description],[CompanyName],[Email],[AlternateEmail],[PhoneNumber]," +
                "[TaxNumber],[IsVatApplicable],[Currency],[PaymentType],[IsCreditAllowed],[BillingAddressId]," +
                "[UseBillingAsShipping],[ShippingAddressId]) " +
                "VALUES (0,@Name,@Description,@CompanyName,@Email,@AlternateEmail,@PhoneNumber," +
                "@TaxNumber,@IsVatApplicable,@Currency,@PaymentType,@IsCreditAllowed,@BillingAddressId," +
                "@UseBillingAsShipping,@ShippingAddressId); " +
                "SELECT SCOPE_IDENTITY()", dealerAccount);
        
        public List<DealerAccount> GetDealerAccounts() =>
            _connection.Query<DealerAccount>("SELECT * FROM[DealerAccount] ")
                .ToList();

        public DealerAccount GetDealerAccount(long dealerAccountId) =>
            _connection.Query<DealerAccount, Address, Address, DealerAccount>(
                "SELECT [DealerAccount].*, Billing.*, Shipping.* " +
                "FROM[DealerAccount] " +
                "LEFT JOIN[Address] AS Billing " +
                "ON Billing.Id = [DealerAccount].BillingAddressId AND Billing.Id " +
                "LEFT JOIN[Address] AS Shipping " +
                "ON Shipping.Id = [DealerAccount].BillingAddressId " +
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
                "[Name]=@Name,[Description]=@Description,[CompanyName]=@CompanyName,[Email]=@Email," +
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
