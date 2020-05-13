using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Services.StoreCustomerServices.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Services.StoreCustomerServices {
    public class StoreCustomerService : IStoreCustomerService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IStoreRepositoriesFactory _storeRepositoriesFactory;

        /// <summary>
        ///     ctor().
        /// </summary>
        public StoreCustomerService(IDbConnectionFactory connectionFactory, IStoreRepositoriesFactory storeRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _storeRepositoriesFactory = storeRepositoriesFactory;
        }
    }
}
