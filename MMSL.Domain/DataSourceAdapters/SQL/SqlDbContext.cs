using Microsoft.EntityFrameworkCore;
using MMSL.Domain.DataSourceAdapters.SQL.Contracts;

namespace MMSL.Domain.DataSourceAdapters.SQL
{
    public class SqlDbContext : ISqlDbContext
    {
        public DbContext DbContext { get; }

        public SqlDbContext(DbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
