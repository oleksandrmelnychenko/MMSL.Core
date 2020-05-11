using System.Data;
using MMSL.Domain.Repositories.Identity.Contracts;

namespace MMSL.Domain.Repositories.Identity {
    public class IdentityRolesRepository : IIdentityRolesRepository {
        private readonly IDbConnection _connection;

        public IdentityRolesRepository(IDbConnection connection) {
            _connection = connection;
        }

    }
}
