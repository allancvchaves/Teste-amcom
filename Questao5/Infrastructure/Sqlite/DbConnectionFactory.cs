using System.Data;
using Microsoft.Data.Sqlite;

namespace Questao5.Infrastructure.Sqlite
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(DatabaseConfig config)
        {
            _connectionString = config.Name;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
