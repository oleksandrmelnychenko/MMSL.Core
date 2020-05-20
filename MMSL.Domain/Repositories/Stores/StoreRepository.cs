using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Stores;
using MMSL.Domain.Repositories.Stores.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.Stores {
    public class StoreRepository : IStoreRepository {

        private readonly IDbConnection _connection;

        public StoreRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<Store> GetAll(string searchPhrase) {
            List<Store> stores = _connection.Query<Store>(
                "SELECT *" +
                "FROM Stores " +
                "WHERE IsDeleted = 0 AND PATINDEX('%' + @SearchTerm + '%', [Stores].Name) > 0",
                new { SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase }).ToList();
            return stores;
        }

        public List<Store> GetAllByDealerAccountId(long dealerAccountId, string searchPhrase) =>
            _connection.Query<Store, Address, Store>(
                "SELECT [Stores].*" +
                ",(SELECT COUNT([StoreCustomers].Id) FROM [StoreCustomers] WHERE [StoreCustomers].StoreId = [Stores].Id AND [StoreCustomers].IsDeleted = 0) AS [StoreCustomersCount]" +
                ",[Address].* " +
                "FROM [Stores] " +
                "LEFT JOIN [StoreMapDealerAccounts] ON [StoreMapDealerAccounts].StoreId = [Stores].Id " +
                "LEFT JOIN [Address] ON [Address].Id = [Stores].AddressId " +
                "WHERE [StoreMapDealerAccounts].DealerAccountId = @Id AND [Stores].IsDeleted = 0" +
                "AND PATINDEX('%' + @SearchTerm + '%', [Stores].Name) > 0",
                (store, address) => {
                    if (store != null) {
                        store.Address = address;
                    }
                    return store;
                },
                new {
                    Id = dealerAccountId,
                    SearchTerm = string.IsNullOrEmpty(searchPhrase) ? string.Empty : searchPhrase
                }).ToList();

        public Store NewStore(Store store, long dealerAccountId) =>
            _connection.Query<Store, Address, Store>(
               "INSERT INTO [Stores] (IsDeleted,[Name],[ContactEmail],[BillingEmail],[AddressId]) " +
               "VALUES(0,@Name,@ContactEmail,@BillingEmail,@AddressId) " +

               "DECLARE @NewStoreId bigint = SCOPE_IDENTITY()" +
               "INSERT INTO [StoreMapDealerAccounts] (IsDeleted,[DealerAccountId],[StoreId])" +
               "VALUES(0,@DealerAccountId,@NewStoreId) " +

               "SELECT [Stores].*, [Address].* " +
               "FROM [Stores] " +
               "LEFT JOIN [Address] ON [Address].Id = [Stores].AddressId " +
               "WHERE [Stores].Id = @NewStoreId",
               (store, address) => {
                   store.Address = address;
                   return store;
               },
               new {
                   Name = store.Name,
                   AddressId = store.AddressId,
                   DealerAccountId = dealerAccountId,
                   ContactEmail = store.ContactEmail,
                   BillingEmail = store.BillingEmail
               }
           ).SingleOrDefault();

        public void UpdateStore(Store store) =>
             _connection.Query<Store>(
                "UPDATE [Stores]" +
                "SET [IsDeleted]=@IsDeleted,[LastModified]=getutcdate()," +
                "[Name]=@Name,[Description]=@Description," +
                "[AddressId]=@AddressId,[ContactEmail]=@ContactEmail," +
                "[BillingEmail]=@BillingEmail " +
                "WHERE [Stores].Id=@Id;",
                store);

        public Store GetStoreById(long storeId) =>
             _connection.Query<Store, Address, Store>(
               "SELECT [Stores].*, [Address].* " +
               "FROM [Stores] " +
               "LEFT JOIN [Address] ON [Address].Id = [Stores].AddressId " +
               "WHERE [Stores].Id = @Id ",
               (store, address) => {
                   store.Address = address;
                   return store;
               },
               new {
                   Id = storeId
               }
           ).SingleOrDefault();
    }
}
