using Dapper;
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
            var bankDetails =_connection.Query<Store>(
                "SELECT *" +
                "FROM Stores " +
                "WHERE IsDeleted = 0").ToList();
            return bankDetails;
        }
    }
}
