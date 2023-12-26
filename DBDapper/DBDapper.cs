using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// A utility class for interacting with a SQL Server database using Dapper.
/// </summary>
public class DBDapper : IDisposable
{
    private readonly string? _connectionString;

    /// <summary>
    /// Initializes a new instance of the DBDapper class.
    /// </summary>
    /// <param name="connectionStringName">The name of the connection string in the appsettings.json file.</param>
    public DBDapper(string connectionStringName = "SqlConnection")
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        _connectionString = configuration.GetConnectionString(connectionStringName);
    }

    /// <summary>
    /// Closes and disposes of the database connection.
    /// </summary>
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

    /// <summary>
    /// Gets the database connection string.
    /// </summary>
    public string? ConnectionString => _connectionString;


    /// <summary>
    /// Executes a stored procedure and returns the results as a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="sp">The name of the stored procedure to execute.</param>
    /// <param name="parameters">Parameters to pass to the stored procedure.</param>
    /// <returns>A list of objects returned by the stored procedure.</returns>
    public List<T> RunSqlProc<T>(string sp, object? parameters = null)
    {
        using IDbConnection db = GetOpenConnection();
        return db.Query<T>(sp, parameters, commandType: CommandType.StoredProcedure).ToList();
    }

    /// <summary>
    /// Asynchronously executes a stored procedure and returns the results as a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="sp">The name of the stored procedure to execute.</param>
    /// <param name="parameters">Parameters to pass to the stored procedure.</param>
    /// <returns>A list of objects returned by the stored procedure.</returns>
    public async Task<List<T>> RunSqlProcAsync<T>(string sp, object? parameters = null)
    {
        using IDbConnection db = GetOpenConnection();
        return (await db.QueryAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure)).ToList();
    }

    /// <summary>
    /// Executes a SQL query and returns the results as a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Parameters to pass to the query.</param>
    /// <returns>A list of objects returned by the SQL query.</returns>
    public List<T> RunSqlQuery<T>(string query, object? parameters = null)
    {
        using IDbConnection db = GetOpenConnection();
        return db.Query<T>(query, parameters, commandType: CommandType.Text).ToList();
    }

    /// <summary>
    /// Asynchronously executes a SQL query and returns the results as a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Parameters to pass to the query.</param>
    /// <returns>A list of objects returned by the SQL query.</returns>
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
