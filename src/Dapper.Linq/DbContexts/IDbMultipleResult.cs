using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// DataReader multiple result sets
    /// </summary>
    public interface IDbMultipleResult : IDisposable
    {
        /// <summary>
        /// Returns the current dynamic type result set
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetList();
        /// <summary>
        /// Asynchronously returns the current dynamic type result set
        /// </summary>
        /// <returns></returns>
        Task<List<dynamic>> GetListAsync();
        /// <summary>
        /// Returns the current T result set
        /// </summary>
        /// <typeparam name="T">Result set type</typeparam>
        /// <returns></returns>
        List<T> GetList<T>();
        /// <summary>
        /// Asynchronously returns the current T type result set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>();
        /// <summary>
        /// Returns the current dynamic type result
        /// </summary>
        /// <returns></returns>
        object Get();
        /// <summary>
        /// Asynchronously returns the current dynamic type result
        /// </summary>
        /// <returns></returns>
        Task<object> GetAsync();
        /// <summary>
        /// Returns the current T type result
        /// </summary>
        /// <typeparam name="T">Result set type</typeparam>
        /// <returns></returns>
        T Get<T>();
        /// <summary>
        /// Asynchronously returns the current T type result
        /// </summary>
        /// <typeparam name="T">Result set type</typeparam>
        /// <returns></returns>
        Task<T> GetAsync<T>();
    }
}