using System.Data;
using System.Data.SqlClient;
using MMSL.Common;

namespace MMSL.Domain.DbConnectionFactory
{
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection NewSqlConnection()
        {
            return new SqlConnection(ConfigurationManager.DatabaseConnectionString);
        }
    }
}
