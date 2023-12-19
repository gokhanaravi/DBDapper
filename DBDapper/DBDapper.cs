using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DBDapper
{
    public class DBDapper : IDisposable
    {
        private readonly string? _connectionString;

        public DBDapper(string connectionStringName = "SqlConnection")
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _connectionString = configuration.GetConnectionString(connectionStringName);
        }

        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
            GC.SuppressFinalize(this);
        }

        private IDbConnection Connection
        {
            get
            {
                IDbConnection connection = new SqlConnection(_connectionString!);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return connection;
            }
        }

        public string? ConnectionString => _connectionString;

        public List<T> RunSqlProc<T>(string sp, object? parameters = null)
        {
            using IDbConnection db = GetOpenConnection();
            return db.Query<T>(sp, parameters, commandType: CommandType.StoredProcedure).ToList();
        }

        public async Task<List<T>> RunSqlProcAsync<T>(string sp, object? parameters = null)
        {
            using IDbConnection db = GetOpenConnection();
            return (await db.QueryAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure)).ToList();
        }

        public List<T> RunSqlQuery<T>(string query, object? parameters = null)
        {
            using IDbConnection db = GetOpenConnection();
            return db.Query<T>(query, parameters, commandType: CommandType.Text).ToList();
        }

        public async Task<List<T>> RunSqlQueryAsync<T>(string query, object? parameters = null)
        {
            using IDbConnection db = GetOpenConnection();
            var result = await db.QueryAsync<T>(query, parameters, commandType: CommandType.Text);
            return result.ToList();
        }

        private SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(_connectionString!);
            connection.Open();
            return connection;
        }
    }
}
