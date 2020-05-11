using System.Data;
using Dapper;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.Repositories.Identity.Contracts;

namespace MMSL.Domain.Repositories.Identity {
    public class AccountTypeCompanyInfoRepository : IAccountTypeCompanyInfoRepository {

        private readonly IDbConnection _connection;

        public AccountTypeCompanyInfoRepository(IDbConnection connection) {
            _connection = connection;
        }

        public AccountTypeCompanyInfo GetAccountTypeCompanyInfo(long companyInfoId) =>
            _connection.QuerySingleOrDefault<AccountTypeCompanyInfo>(
                "SELECT [AccountTypeCompanyInfo].* " +
                "FROM [AccountTypeCompanyInfo] " +
                "WHERE [AccountTypeCompanyInfo].Id = @Id",
                new {Id = companyInfoId });

        public long AddAccountTypeCompanyInfo(AccountTypeCompanyInfo companyInfo) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [AccountTypeCompanyInfo] " +
                "([IsDeleted], [Name], [Description], [City], [Address]) " +
                "VALUES (0, @Name, @Description, @City, @Address); " +
                "SELECT SCOPE_IDENTITY()",
                companyInfo);

        public void UpdateAccountTypeCompanyInfo(AccountTypeCompanyInfo companyInfo) =>
            _connection.QuerySingleOrDefault<AccountTypeCompanyInfo>(
                "UPDATE [AccountTypeCompanyInfo] " +
                "SET IsDeleted = @IsDeleted, [Name] = @Name, [Description] = @Description, " +
                "[City] = @City, [Address] = @Address, LastModified = getutcdate() " +
                "WHERE [AccountTypeCompanyInfo].Id = @Id; " +
                "SELECT SCOPE_IDENTITY()",
                companyInfo);
    }
}
