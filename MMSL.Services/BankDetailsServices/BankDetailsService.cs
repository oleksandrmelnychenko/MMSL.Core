using MMSL.Domain.DataContracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.BankDetails;
using MMSL.Domain.Repositories.BankDetails.Contracts;
using MMSL.Services.BankDetailsServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.BankDetailsServices {
    public class BankDetailsService : IBankDetailsService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IBankDetailRepositoriesFactory _bankDetailRepositoriesFactory;

        public BankDetailsService(IDbConnectionFactory connectionFactory, IBankDetailRepositoriesFactory bankDetailRepositoriesFactory) {
            _connectionFactory = connectionFactory;
            _bankDetailRepositoriesFactory = bankDetailRepositoriesFactory;
        }

        public Task<List<BankDetail>> GetAllBankDetailsAsync() =>
            Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection()) {
                    List<BankDetail> bankDetails = null;
                    IBankDetailRepository bankDetailRepository = _bankDetailRepositoriesFactory.NewBankDetailRepository(connection);
                    bankDetails = bankDetailRepository.GetAll();
                    return bankDetails;
                }
            });

        public Task<BankDetail> NewBankDetail(NewBankDetailDataContract newBankDetailDataContract) {
            throw new NotImplementedException();
        }
    }
}
