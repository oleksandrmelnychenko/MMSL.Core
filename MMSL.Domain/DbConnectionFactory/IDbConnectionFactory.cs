using System.Data;

namespace MMSL.Domain.DbConnectionFactory
{
    public interface IDbConnectionFactory
    {
        IDbConnection NewSqlConnection();
    }
}
