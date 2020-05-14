using MMSL.Domain.Entities.PaymentTypes;
using System.Collections.Generic;

namespace MMSL.Domain.Repositories.Types.Contracts {
    public interface IPaymentTypeRepository {
        List<PaymentType> GetPaymentTypes();
        PaymentType GetPaymentType(long id);
        long AddPaymentType(PaymentType paymentType);
        PaymentType UpdatePaymentType(PaymentType paymentType);
    }
}
