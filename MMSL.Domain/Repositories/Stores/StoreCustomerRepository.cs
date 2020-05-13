using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.StoreCustomers;
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

        public List<StoreCustomer> GetStoreCustomers(long storeId) =>
            _connection.Query<StoreCustomer>(
                "SELECT [StoreCustomers].* " +
                "FROM [StoreCustomers] " +
                "WHERE [StoreCustomers].[StoreId] = @Id AND [StoreCustomers].[IsDeleted] = 0",
                new {
                    Id = storeId
                })
            .ToList();

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
