using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TestProject1
{
    public class DbFixture : IDisposable
    {
        public string ConnectionString { get; }

        private readonly string _masterConnectionString;
        private readonly string _databaseName;

        public DbFixture() :this(new TestConfiguration()) {}

        public DbFixture(TestConfiguration config)
        {
            _databaseName = Guid.NewGuid().ToString();
            _masterConnectionString = GetConnectionStringToDatabase(config.SqlServerConnectionString, "master");

            CreateDatabase(_databaseName, _masterConnectionString).Wait();
            ConnectionString = GetConnectionStringToDatabase(_masterConnectionString, _databaseName);
        }

        private async Task CreateDatabase(string databaseName, string mainConnectionString)
        {
            using var connection = new SqlConnection(mainConnectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand($"CREATE DATABASE [{databaseName}]", connection);
            await command.ExecuteNonQueryAsync();
        }
        
        private async Task DeleteDatabase(string databaseName, string mainConnectionString)
        {
            using var connection = new SqlConnection(mainConnectionString);
            await connection.OpenAsync();
            using var dropAllConnections = new SqlCommand($"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", connection);
            using var dropDatabaseCommand = new SqlCommand($"DROP DATABASE [{databaseName}]", connection);
            dropAllConnections.ExecuteNonQuery();
            dropDatabaseCommand.ExecuteNonQuery();
        }

        private string GetConnectionStringToDatabase(string mainConnectionString, string database)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(mainConnectionString);
            connectionStringBuilder.InitialCatalog = database;
            return connectionStringBuilder.ToString();
        }

        public async Task ClearDatabase()
        {
            using var connection = new SqlConnection(_masterConnectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("EXEC sp_MSForEachTable @command1='DELETE FROM ?'", connection);
            await command.ExecuteNonQueryAsync();
        }
        

        public void Dispose()
        {
            //DeleteDatabase(_databaseName, _masterConnectionString).Wait();
        }
    }
}