using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DBDapper : IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;

    public DBDapper(IConfiguration configuration, string connectionStringName = "SqlConnection")
    {
        _connectionString = configuration.GetConnectionString(connectionStringName);
    }

    public void Dispose()
    {
        _connection?.Close();
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

    public SqlConnection GetOpenConnection() => new SqlConnection(_connectionString);

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
}
