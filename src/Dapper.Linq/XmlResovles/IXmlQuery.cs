using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// xml command mapper
    /// </summary>
    public interface IXmlQuery
    {
        /// <summary>
        /// Execute a multi-result set query and return IMultiResult
        /// </summary>
        /// <param name="commandTimeout">overtime time</param>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        IDbMultipleResult MultipleQuery(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a single result set query and return a result set of dynamic type
        /// </summary>
        /// <param name="commandTimeout">overtime time</param>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        IEnumerable<dynamic> Query(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Asynchronously executes a single result set query and returns a dynamic type result set
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryAsync(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a single result set query and return a result set of type T
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Asynchronously executes a single result set query and returns a result set of type T
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Executes a query without a result set and returns the number of rows affected
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        int Execute(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Executes a query without a result set asynchronously and returns the number of rows affected
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Execute a query without a result set and return data of the specified type
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        T ExecuteScalar<T>(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Executes a query without a result set asynchronously and returns the specified type of data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Add database parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void AddDbParameter(string name, object value);
    }
}
