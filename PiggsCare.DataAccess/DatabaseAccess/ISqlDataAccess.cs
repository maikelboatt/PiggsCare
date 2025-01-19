namespace PiggsCare.DataAccess.DatabaseAccess
{
    public interface ISqlDataAccess
    {
        /// <summary>
        ///     Gets records from database
        /// </summary>
        /// <param name="storedProcedure" > Group of SQL statements that are stored together in a database</param>
        /// <param name="parameters" >Arguments passed to the stored procedure</param>
        /// <param name="connectionString" >string that contains information about a data source</param>
        /// <typeparam name="T" >data type of model</typeparam>
        /// <typeparam name="TU" >data type of arguments</typeparam>
        /// <returns>Either a collection of instance of the model</returns>
        Task<IEnumerable<T>> QueryAsync<T, TU>( string storedProcedure, TU parameters, string connectionString = "DefaultConnection" );

        /// <summary>
        ///     Modifies database record
        /// </summary>
        /// <param name="storedProcedure" >Group of SQL statements that are stored together in a database</param>
        /// <param name="parameters" >Arguments passed to the stored procedure</param>
        /// <param name="connectionString" ></param>
        /// <typeparam name="T" >data type of arguments</typeparam>
        Task CommandAsync<T>( string storedProcedure, T parameters, string connectionString = "DefaultConnection" );
    }
}
