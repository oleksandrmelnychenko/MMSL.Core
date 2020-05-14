using MMSL.Domain.Entities.PaymentTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.Types.Contracts {
    public interface IPaymentTypeService {
        Task<List<PaymentType>> GetPaymentTypesAsync();
        Task<PaymentType> AddPaymentTypeAsync(PaymentType paymentType);
        Task<PaymentType> UpdatePaymentTypeAsync(PaymentType paymentType);
        Task<PaymentType> DeletePaymentTypeAsync(long paymentTypeId);
    }
}
