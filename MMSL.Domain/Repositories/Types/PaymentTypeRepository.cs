using Dapper;
using MMSL.Domain.Entities.PaymentTypes;
using MMSL.Domain.Repositories.Types.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Types {
    class PaymentTypeRepository : IPaymentTypeRepository {
        private IDbConnection _connection;

        public PaymentTypeRepository(IDbConnection connection) {
            this._connection = connection;
        }

        public long AddPaymentType(PaymentType paymentType) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [PaymentTypes] " +
                "([IsDeleted], [Created], [Name], [Description]) " +
                "VALUES (0, @Created, @Name, @Description);" +
                "SELECT SCOPE_IDENTITY()",
                paymentType);

        public PaymentType GetPaymentType(long id) =>
            _connection.QuerySingleOrDefault<PaymentType>(
                "SELECT * FROM [PaymentTypes] " +
                "WHERE [PaymentTypes].Id = @Id",
                new { Id = id });

        public List<PaymentType> GetPaymentTypes() =>
            _connection.Query<PaymentType>("SELECT * FROM [PaymentTypes]").ToList();

        public PaymentType UpdatePaymentType(PaymentType paymentType) =>
            _connection.QuerySingleOrDefault<PaymentType>(
                "UPDATE [PaymentTypes] SET " +
                "[IsDeleted] = @IsDeleted, [LastModified]=getutcdate(), " +
                "[Name] = @Name, [Description] = @Description " +
                "WHERE [PaymentTypes].Id = @Id",
                paymentType);
    }
}
