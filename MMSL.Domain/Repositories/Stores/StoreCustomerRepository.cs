using MMSL.Domain.Repositories.Stores.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
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

    }
}
