using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MMSL.Domain.Entities.Identity;
using MMSL.Domain.Repositories.Identity.Contracts;

namespace MMSL.Domain.Repositories.Identity {
    public class AccountTypeRepository : IAccountTypeRepository {

        private readonly IDbConnection _connection;

        public AccountTypeRepository(IDbConnection connection) {
            _connection = connection;
        }

        public AccountType GetAccountType(long accountTypeId) {
            return _connection.Query<AccountType, AccountTypeCompanyInfo, AccountType>(
                "SELECT [AccountType].*, " +
                "[AccountTypeCompanyInfo].* " +
                "FROM [AccountType] " +
                "LEFT JOIN [AccountTypeCompanyInfo] ON [AccountType].CompanyInfoId = [AccountTypeCompanyInfo].Id " +
                "WHERE [AccountType].Id = @Id ",
                (accountType, companyInfo) => {
                    if (companyInfo != null) {
                        accountType.CompanyInfo = companyInfo;
                    }

                    return accountType;
                },
                new { Id = accountTypeId })
            .SingleOrDefault();
        }

        public long AddAccountType(AccountType companyInfo) =>
            _connection.QuerySingleOrDefault<long>(
                "INSERT INTO [AccountType] " +
                "([IsDeleted], [UserIdentityId], [UserAccountType], [CompanyInfoId]) " +
                "VALUES (0, @UserIdentityId, @UserAccountType, @CompanyInfoId); " +
                "SELECT SCOPE_IDENTITY() ",
                companyInfo);

        public void UpdateAccountType(AccountType companyInfo) =>
            _connection.Query<AccountType>(
                "UPDATE [AccountType] " +
                "SET IsDeleted = @IsDeleted, [UserIdentityId] = @UserIdentityId, [UserAccountType] = @UserAccountType, [CompanyInfoId] = @CompanyInfoId, LastModified = getutcdate() " +
                "WHERE [AccountType].Id = @Id;" +
                "SELECT SCOPE_IDENTITY()",
                companyInfo);

        public IEnumerable<AccountType> GetAccountTypes(long userIdentityId, AccountTypes? targetType = null) {
            return _connection.Query<AccountType, AccountTypeCompanyInfo, AccountType>(
                "SELECT [AccountType].*, " +
                "[AccountTypeCompanyInfo].* " +
                "FROM [AccountType] " +
                "LEFT JOIN [AccountTypeCompanyInfo] ON [AccountType].CompanyInfoId = [AccountTypeCompanyInfo].Id " +
                "AND [AccountTypeCompanyInfo].[IsDeleted] = 0 " +
                "WHERE [AccountType].UserIdentityId = @Id " +
                "AND [AccountType].[IsDeleted] = 0" +
                (
                    targetType.HasValue
                        ? $"AND [AccountType].UserAccountType = '{(int)targetType.Value}'"
                        : string.Empty
                ),
                (accountType, companyInfo) => {
                    if (companyInfo != null) {
                        accountType.CompanyInfo = companyInfo;
                    }

                    return accountType;
                },
                new { Id = userIdentityId })
            .ToList();
        }
    }
}
