using MMSL.Domain.Entities.BankDetails;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.Repositories.BankDetails.Contracts {
    public interface IBankDetailRepository {
        List<BankDetail> GetAll();
    }
}
