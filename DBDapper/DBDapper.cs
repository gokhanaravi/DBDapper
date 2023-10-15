using Dapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DBDapper : IDisposable
{
    private string _connectionString;
    private IDbConnection _connection;

    public DBDapper(string? ConnectionStringName = "SqlConnection")
    {
        _connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build().GetConnectionString(ConnectionStringName);
    }
    public void Dispose()
    {
        if (_connection != null && _connection.State != ConnectionState.Closed)
            _connection.Close();
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

    public string ConnectionString
    {
        get { return _connectionString; }
    }

    public SqlConnection GetOpenConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }


    public List<T> RunSqlProc<T>(string sp, dynamic obj = null)
    {
        using IDbConnection db = GetOpenConnection();
        {
            if (obj != null)
            {
                return db.Query<T>(sp, new RouteValueDictionary(obj).ToDictionary(item => "@" + item.Key, item => item.Value), commandType: CommandType.StoredProcedure).ToList();
            }
            else
            {
                return db.Query<T>(sp).ToList();
            }

        }
    }
}