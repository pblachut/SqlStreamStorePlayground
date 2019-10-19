using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlStoreTest
{
    public interface IConnectionFactory
    {
        string ConnectionString { get; }
        Task<SqlConnection> CreateAsync();

        SqlConnection Create();
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString => _connectionString;

        public async Task<SqlConnection> CreateAsync()
        {
            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            return connection;
        }

        public SqlConnection Create()
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            return connection;
        }
    }
}