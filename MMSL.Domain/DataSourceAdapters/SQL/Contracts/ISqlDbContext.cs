using Microsoft.EntityFrameworkCore;

namespace MMSL.Domain.DataSourceAdapters.SQL.Contracts
{
    public interface ISqlDbContext
    {
        DbContext DbContext { get; }
    }
}
