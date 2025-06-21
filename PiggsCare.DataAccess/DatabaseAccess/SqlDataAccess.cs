using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace PiggsCare.DataAccess.DatabaseAccess
{
    /// <summary>
    ///     Provides methods for executing commands and queries against a SQL database using Dapper.
    /// </summary>
    /// <param name="connectionString" >The connection string to the SQL database.</param>
    /// <param name="logger" >The logger instance for logging errors.</param>
    public class SqlDataAccess( string connectionString, ILogger<SqlDataAccess> logger ):ISqlDataAccess
    {
        /// <summary>
        ///     Executes a stored procedure command asynchronously.
        /// </summary>
        /// <typeparam name="T" >The type of the parameters object.</typeparam>
        /// <param name="storedProcedure" >The name of the stored procedure to execute.</param>
        /// <param name="parameters" >The parameters to pass to the stored procedure.</param>
        public async Task CommandAsync<T>( string storedProcedure, T parameters )
        {
            try
            {
                await using SqlConnection connection = new(connectionString);
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                // Log the exception using Serilog
                logger.LogError(e, "Error executing command: {Message}", e.Message);
                throw;
            }
        }

        /// <summary>
        ///     Executes a stored procedure query asynchronously and returns the result set.
        /// </summary>
        /// <typeparam name="T" >The type of the result objects.</typeparam>
        /// <typeparam name="TU" >The type of the parameters object.</typeparam>
        /// <param name="storedProcedure" >The name of the stored procedure to execute.</param>
        /// <param name="parameters" >The parameters to pass to the stored procedure.</param>
        /// <returns>An enumerable of result objects of type <typeparamref name="T" />.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T, TU>( string storedProcedure, TU parameters )
        {
            try
            {
                await using SqlConnection connection = new(connectionString);
                return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                // Log the exception using Serilog
                logger.LogError(e, "Error executing query: {Message}", e.Message);
                throw;
            }
        }
    }
}
