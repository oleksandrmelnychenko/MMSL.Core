using MMSL.Domain.DataContracts.Types;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.PaymentTypes;
using MMSL.Domain.Repositories.Types.Contracts;
using MMSL.Services.Types.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMSL.Services.Types {
    public class PaymentTypeService : IPaymentTypeService {

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ITypesRepositoriesFactory _typesRepositoriesFactory;

        public PaymentTypeService(ITypesRepositoriesFactory typesRepositoriesFactory, IDbConnectionFactory connectionFactory) {
            _typesRepositoriesFactory = typesRepositoriesFactory;
            _connectionFactory = connectionFactory;
        }

        public Task<PaymentType> AddPaymentTypeAsync(PaymentTypeDataContract paymentType)  =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    PaymentType payment = paymentType.GetEntity();

                    payment.Id = _typesRepositoriesFactory
                        .NewPaymentTypeRepository(connection)
                        .AddPaymentType(paymentType.GetEntity());

                    return payment;
                }
            });

        public Task<PaymentType> DeletePaymentTypeAsync(long paymentTypeId) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    IPaymentTypeRepository repository = _typesRepositoriesFactory.NewPaymentTypeRepository(connection);

                    PaymentType paymentType = repository.GetPaymentType(paymentTypeId);

                    paymentType.IsDeleted = true;

                    repository.UpdatePaymentType(paymentType);

                    return paymentType;
                }
            });

        public Task<List<PaymentType>> GetPaymentTypesAsync() =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _typesRepositoriesFactory
                        .NewPaymentTypeRepository(connection)
                        .GetPaymentTypes();
                }
            });

        public Task<PaymentType> UpdatePaymentTypeAsync(PaymentTypeDataContract paymentType) =>
            Task.Factory.StartNew(() => {
                using (var connection = _connectionFactory.NewSqlConnection()) {
                    return _typesRepositoriesFactory
                         .NewPaymentTypeRepository(connection)
                         .UpdatePaymentType(paymentType.GetEntity());
                }
            });
    }
}
