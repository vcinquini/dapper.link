using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// linq query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDbQuery<T>
    {
        /// <summary>
        /// Retrieve data by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(object id);
        /// <summary>
        /// Retrieve data asynchronously by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAsync(object id);
        /// <summary>
        /// count query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        int Count(int? commandTimeout = null);
        /// <summary>
        /// Asynchronous count query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<int> CountAsync(int? commandTimeout = null);
        /// <summary>
        /// count query
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="expression">Field List</param>
        /// <returns></returns>
        int Count<TResult>(Expression<Func<T, TResult>> expression);
        /// <summary>
        /// Asynchronous count query
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="expression">Field List</param>
        /// <returns></returns>
        Task<int> CountAsync<TResult>(Expression<Func<T, TResult>> expression);
        /// <summary>
        /// delete query
        /// </summary>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int Delete(int? commandTimeout = null);
        /// <summary>
        /// Asynchronous delete query
        /// </summary>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(int? commandTimeout = null);
        /// <summary>
        /// delete query
        /// </summary>
        /// <param name="expression">query condition</param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Asynchronous delete query
        /// </summary>
        /// <param name="expression">query condition</param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        /// exists query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        bool Exists(int? commandTimeout = null);
        /// <summary>
        /// Asynchronous exists query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int? commandTimeout = null);
        /// <summary>
        /// exists query
        /// </summary>
        /// <param name="expression">query condition</param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Asynchronous exists query
        /// </summary>
        /// <param name="expression">query condition</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        /// update query, if no where is specified, it will be applied to all records
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        int Update(int? commandTimeout = null);
        /// <summary>
        /// Asynchronous update query, if no where is specified, it will be applied to all records
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<int> UpdateAsync(int? commandTimeout = null);
        /// <summary>
        /// update query, the default is updated according to Primarkey, if there is where, only the specified update condition is used,
        /// The primary key field and primary key field cannot be updated through this interface
        /// </summary>
        /// <param name="entity">parameter</param>
        /// <returns></returns>
        int Update(T entity);
        /// <summary>
        /// Asynchronous update query, updated according to Primarkey by default, if there is where, only the specified update condition is used,
        /// The primary key field and primary key field cannot be updated through this interface
        /// </summary>
        /// <param name="entity">parameter</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity);
        /// <summary>
        /// insert query, this interface will ignore the identity field
        /// </summary>
        /// <param name="entity">parameter</param>
        /// <returns></returns>
        int Insert(T entity);
        /// <summary>
        /// Asynchronous insert query, this interface will ignore the identity field
        /// </summary>
        /// <param name="entity">parameter</param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity);
        /// <summary>
        /// insert query and return id, this interface will ignore the identity field
        /// </summary>
        /// <param name="entity">parameter</param>
        /// <returns></returns>
        int InsertReturnId(T entity);
        /// <summary>
        /// Asynchronous insert query and return id, the interface will ignore the identity field
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertReturnIdAsync(T entity);
        ///<summary>
        /// Batch insert query, this interface will ignore the identity field
        /// </summary>
        /// <param name="entitys">Parameter collection</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        int Insert(IEnumerable<T> entities, int? commandTimeout = null);
        /// <summary>
        /// Asynchronous batch insert query, this interface will ignore the identity field
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<int> InsertAsync(IEnumerable<T> entities, int? commandTimeout = null);
        /// <summary>
        /// select query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        IEnumerable<T> Select(int? commandTimeout = null);
        /// <summary>
        /// Asynchronous select query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<IEnumerable<T>> SelectAsync(int? commandTimeout = null);
        /// <summary>
        /// Paging select query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns>Result set, total records</returns>
        (IEnumerable<T>, int) SelectMany(int? commandTimeout = null);
        /// <summary>
        /// Asynchronous paging select query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<(IEnumerable<T>, int)> SelectManyAsync(int? commandTimeout = null);
        /// <summary>
        /// select query
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        IEnumerable<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        /// <summary>
        /// Asynchronous select query
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        /// <summary>
        /// Paging select query
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        (IEnumerable<TResult>, int) SelectMany<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        /// <summary>
        /// Asynchronous paging select query
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<(IEnumerable<TResult>, int)> SelectManyAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        /// <summary>
        /// select query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        T Single(int? commandTimeout = null);
        /// <summary>
        /// Asynchronous select query
        /// </summary>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<T> SingleAsync(int? commandTimeout = null);
        /// <summary>
        /// select query
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        TResult Single<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        /// <summary>
        /// Asynchronous select query
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<TResult> SingleAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        /// <summary>
        /// Filter fields when insert, update, select
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="expression">Field List</param>
        /// <returns></returns>
        IDbQuery<T> Filter<TResult>(Expression<Func<T, TResult>> expression);
        /// <summary>
        /// set query
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <param name="condition">Is it valid</param>
        /// <returns></returns>
        IDbQuery<T> Set<TResult>(Expression<Func<T, TResult>> column, TResult value, bool condition = true);
        /// <summary>
        /// set query
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="column">field</param>
        /// <param name="expression">expression</param>
        /// <param name="condition">Is it valid</param>
        /// <returns></returns>
        IDbQuery<T> Set<TResult>(Expression<Func<T, TResult>> column, Expression<Func<T, TResult>> expression, bool condition = true);
        /// <summary>
        /// take query, get count records from the row with subscript 0
        /// </summary>
        /// <param name="count">Number of records</param>
        /// <param name="condition">condition</param>
        /// <returns></returns>
        IDbQuery<T> Take(int count, bool condition = true);
        /// <summary>
        /// skip, get count records from the row with index index
        /// </summary>
        /// <param name="index">Start subscript</param>
        /// <param name="count">Number of records</param>
        /// <param name="condition">condition</param>
        /// <returns></returns>
        IDbQuery<T> Skip(int index, int count, bool condition = true);
        /// <summary>
        /// page query, get count records from the row whose subscript is (index-1)*count
        /// </summary>
        /// <param name="index">Starting page number</param>
        /// <param name="count">Number of records</param>
        /// <param name="condition">condition</param>
        /// <returns></returns>
        IDbQuery<T> Page(int index, int count, bool condition = true);
        /// <summary>
        /// Specifies the read lock
        /// </summary>
        /// <param name="lockname"></param>
        /// <returns></returns>
        IDbQuery<T> With(string lockname);
        /// <summary>
        /// where query, multiple where effectively use and connection
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="condition">Is it valid</param>
        /// <returns></returns>
        IDbQuery<T> Where(Expression<Func<T, bool>> expression, bool condition = true);
        /// <summary>
        /// having query, multiple having queries effectively use and connection
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="condition">Is it valid</param>
        /// <returns></returns>
        IDbQuery<T> Having(Expression<Func<T, bool>> expression, bool condition = true);
        /// <summary>
        /// group query
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="expression">Field List</param>
        /// <returns></returns>
        IDbQuery<T> GroupBy<TResult>(Expression<Func<T, TResult>> expression);
        /// <summary>
        /// orderby query, ascending order
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="expression">Field List</param>
        /// <returns></returns>
        IDbQuery<T> OrderBy<TResult>(Expression<Func<T, TResult>> expression);
        /// <summary>
        /// orderby query, descending order
        /// </summary>
        /// <typeparam name="TResult">Type inference</typeparam>
        /// <param name="expression">Field List</param>
        /// <returns></returns>
        IDbQuery<T> OrderByDescending<TResult>(Expression<Func<T, TResult>> expression);
        /// <summary>
        /// Summation
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        TResult Sum<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        /// <summary>
        /// Asynchronous summation
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="expression">Field List</param>
        /// <param name="commandTimeout">Timeout</param>
        /// <returns></returns>
        Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
    }
}