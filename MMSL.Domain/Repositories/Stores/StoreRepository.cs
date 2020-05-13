﻿using Dapper;
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

        public List<Store> GetAll() {
            List<Store> stores = _connection.Query<Store>(
                "SELECT *" +
                "FROM Stores " +
                "WHERE IsDeleted = 0").ToList();
            return stores;
        }

        public List<Store> GetAllByDealerAccountId(long dealerAccountId) =>
            _connection.Query<Store>(
                "SELECT * " +
                "FROM [Stores] " +
                "WHERE [Stores].DealerAccountId = @Id AND [Stores].IsDeleted = 0",
                new { Id = dealerAccountId }).ToList();

        public Store NewStore(Store store) =>
            _connection.Query<Store, Address, Store>(
               "INSERT INTO [Stores] (IsDeleted,[Name],[ContactEmail],[BillingEmail],[DealerAccountId],[AddressId]) " +
               "VALUES(0,@Name,@ContactEmail,@BillingEmail,@DealerAccountId,@AddressId) " +
               "SELECT [Stores].*, [Address].* " +
               "FROM [Stores] " +
               "LEFT JOIN [Address] ON [Address].Id = [Stores].AddressId " +
               "WHERE [Stores].Id = (SELECT SCOPE_IDENTITY()) ",
               (store, address) => {
                   store.Address = address;
                   return store;
               },
               new {
                   Name = store.Name,
                   AddressId = store.AddressId,
                   DealerAccountId = store.DealerAccountId,
                   ContactEmail = store.ContactEmail,
                   BillingEmail = store.BillingEmail
               }
           ).SingleOrDefault();

        public void UpdateStore(Store store) =>
             _connection.Query<Store>(
                "UPDATE [Stores]" +
                "SET [IsDeleted]=@IsDeleted,[Created]=@Created,[LastModified]=getutcdate()," +
                "[Name]=@Name,[Description]=@Description," +
                "[DealerAccountId]=@DealerAccountId,[AddressId]=@AddressId,[ContactEmail]=@ContactEmail," +
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
