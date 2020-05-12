using MMSL.Domain.DataContracts;
using MMSL.Domain.Entities.BankDetails;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSL.Services.BankDetailsServices.Contracts {
    public interface IBankDetailsService {
        Task<List<BankDetail>> GetAllBankDetailsAsync();

        Task<BankDetail> NewBankDetail(NewBankDetailDataContract newBankDetailDataContract);
    }
}
