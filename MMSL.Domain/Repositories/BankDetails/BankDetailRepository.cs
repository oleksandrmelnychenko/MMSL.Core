using Dapper;
using MMSL.Domain.Entities.BankDetails;
using MMSL.Domain.Repositories.BankDetails.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MMSL.Domain.Repositories.BankDetails {
    public class BankDetailRepository : IBankDetailRepository {

        private readonly IDbConnection _connection;

        public BankDetailRepository(IDbConnection connection) {
            _connection = connection;
        }

        public List<BankDetail> GetAll() {
            var bankDetails =_connection.Query<BankDetail>(
                "SELECT *" +
                "FROM BankDetails" +
                "WHERE IsDeleted = 0").ToList();
            return bankDetails;
        }
    }
}
