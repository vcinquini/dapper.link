using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dapper
{
    public interface IDbContext : IDisposable
    {
        event Logging Logging;
        IDbConnection Connection { get; }
        DbContextType DbContextType { get; }
        IXmlQuery From<T>(string id, T parameter) where T : class;
        IXmlQuery From(string id);
        IDbQuery<T> From<T>();
        void BeginTransaction();
        Task BeginTransactionAsync();
        void BeginTransaction(IsolationLevel level);
        Task BeginTransactionAsync(IsolationLevel level);
        void Close();
        void CommitTransaction();
        IDbMultipleResult QueryMultiple(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        IEnumerable<dynamic> Query(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<dynamic>> QueryAsync(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        IEnumerable<T> Query<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        int Execute(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<int> ExecuteAsync(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        T ExecuteScalar<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null);
        void Open();
        Task OpenAsync();
        void RollbackTransaction();
    }
}
