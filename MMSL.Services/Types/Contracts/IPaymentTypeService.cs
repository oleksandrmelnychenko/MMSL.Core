using MMSL.Domain.DataContracts.Types;
using MMSL.Domain.Entities.PaymentTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.Types.Contracts {
    public interface IPaymentTypeService {
        Task<List<PaymentType>> GetPaymentTypesAsync();
        Task<PaymentType> AddPaymentTypeAsync(PaymentTypeDataContract paymentType);
        Task<PaymentType> UpdatePaymentTypeAsync(PaymentTypeDataContract paymentType);
        Task<PaymentType> DeletePaymentTypeAsync(long paymentTypeId);
    }
}
