using Microsoft.AspNetCore.Mvc;
using MMSL.Domain.DataSourceAdapters.SQL.Contracts;

namespace MMSL.Domain.DataSourceAdapters.SQL
{
    public class SqlContextFactory : ISqlContextFactory
    {
        private readonly ISqlDbContext _sqlDbContext;

        public SqlContextFactory([FromServices] ISqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }

        public ISqlDbContext New()
        {
            return _sqlDbContext;
        }
    }
}
