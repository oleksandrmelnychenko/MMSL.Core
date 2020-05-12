using Dapper;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Repositories.Addresses.Contracts;
using System.Data;
using System.Linq;

namespace MMSL.Domain.Repositories.Addresses {
    public class AddressRepository : IAddressRepository {

        private readonly IDbConnection _connection;

        public AddressRepository(IDbConnection connection) {
            _connection = connection;
        }

        public long AddAddress(Address address) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [Address] " +
                "([IsDeleted], [AddressLine1], [AddressLine2], [City], [State], [Country], [ZipCode]) " +
                "VALUES(0, @AddressLine1, @AddressLine2, @City, @State, @Country, @ZipCode); " +
                "SELECT SCOPE_IDENTITY() ",
                address);

        public Address GetAddress(long addressId) => 
            _connection.Query<Address>(
                "SELECT * FROM [Address] WHERE [Address].Id = @Id ",
                new { Id = addressId })
            .SingleOrDefault();

        public void UpdateAddress(Address address) =>
            _connection.Query<Address>(
                "UPDATE [Address] " +
                "SET IsDeleted = @IsDeleted, " +
                "[AddressLine1] = @AddressLine1, " +
                "[AddressLine2] = @AddressLine2, " +
                "[City] = @City, " +
                "[State] = @State, " +
                "[Country] = @Country, " +
                "[ZipCode] = @ZipCode, " +
                "LastModified = getutcdate() " +
                "WHERE[Address].Id = @Id;" +
                "SELECT SCOPE_IDENTITY()",
                address);
    }
}
