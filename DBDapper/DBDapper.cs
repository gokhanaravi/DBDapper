using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DBDapper : IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;

    public DBDapper(string connectionStringName = "SqlConnection")
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        _connectionString = configuration.GetConnectionString(connectionStringName);
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }

    private IDbConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
            }

            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            return _connection;
        }
    }

    public string ConnectionString => _connectionString;

    public List<T> RunSqlProc<T>(string sp, object parameters = null)
    {
        using IDbConnection db = GetOpenConnection();
        return db.Query<T>(sp, parameters, commandType: CommandType.StoredProcedure).ToList();
    }

    public async Task<List<T>> RunSqlProcAsync<T>(string sp, object parameters = null)
    {
        using IDbConnection db = GetOpenConnection();
        return (await db.QueryAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure)).ToList();
    }

    private SqlConnection GetOpenConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
