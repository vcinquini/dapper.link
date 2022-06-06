using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// database context
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// log processing
        /// </summary>
        event Logging Logging;
        /// <summary>
        /// Database linkage
        /// </summary>
        IDbConnection Connection { get; }
        /// <summary>
        /// Database context type
        /// </summary>
        DbContextType DbContextType { get; }
        /// <summary>
        /// Get an xml executor
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <param name="id">command id</param>
        /// <param name="parameter">parameter</param>
        /// <returns></returns>
        IXmlQuery From<T>(string id, T parameter) where T : class;
        /// <summary>
        /// Get an xml executor
        /// </summary>
        /// <param name="id">command id</param>
        /// <returns></returns>
        IXmlQuery From(string id);
        /// <summary>
        /// Get a linq executor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDbQuery<T> From<T>();
        /// <summary>
        /// Open transaction session
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Asynchronously open the transaction session
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();
        /// <summary>
        /// Open transaction session
        /// </summary>
        /// <param name="level">Transaction isolation level</param>
        void BeginTransaction(IsolationLevel level);
        /// <summary>
        /// Asynchronously open the transaction session
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        Task BeginTransactionAsync(IsolationLevel level);
        /// <summary>
        /// Close the connection and transaction
        /// </summary>
        void Close();
        /// <summary>
        /// Commit the current transaction session
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// Execute a multi-result set query and return IMultiResult
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        IDbMultipleResult QueryMultiple(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Execute a single result set query and return a result set of dynamic type
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Asynchronously executes a single result set query and returns a dynamic type result set
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryAsync(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Execute a single result set query and return a result set of type T
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Asynchronously executes a single result set query and returns a result set of type T
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Executes a query without a result set and returns the number of rows affected
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        int Execute(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Executes a query without a result set asynchronously and returns the number of rows affected
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
		/// <returns></returns>
        Task<int> ExecuteAsync(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Execute a query without a result set and return data of the specified type
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Executes a query without a result set asynchronously and returns the specified type of data
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="sql">sql command</param>
        /// <param name="parameter">parameter</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <param name="commandType">Command Type</param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Open the database connection
        /// </summary>
        void Open();
        /// <summary>
        /// Open the database connection asynchronously
        /// </summary>
        /// <returns></returns>
        Task OpenAsync();
        /// <summary>
        /// Roll back the current transaction session
        /// </summary>
        void RollbackTransaction();
    }
}