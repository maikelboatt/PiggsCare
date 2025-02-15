using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace PiggsCare.DataAccess.DatabaseAccess
{
    public class SqlDataAccess( IConfiguration config ):ISqlDataAccess
    {
        private const string DefaultConnectionName = "DefaultConnection";

        public async Task<IEnumerable<T>> QueryAsync<T, TU>( string storedProcedure, TU parameters, string connectionString = DefaultConnectionName )
        {
            string? output = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(output))
                Console.WriteLine($"Connection {connectionString} not found");
            if (string.IsNullOrWhiteSpace(output))
                Console.WriteLine($"Connection {connectionString} not configured");

            using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionString));
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task CommandAsync<T>( string storedProcedure, T parameters, string connectionString = "DefaultConnection" )
        {
            using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionString));
            await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
