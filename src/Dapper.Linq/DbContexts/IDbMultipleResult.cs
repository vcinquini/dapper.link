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
        /// 异步返回当前dynamic类型结果集
        /// </summary>
        /// <returns></returns>
        Task<List<dynamic>> GetListAsync();
        /// <summary>
        /// 返回当前T结果集
        /// </summary>
        /// <typeparam name="T">结果集类型</typeparam>
        /// <returns></returns>
        List<T> GetList<T>();
        /// <summary>
        ///  异步返回当前T类型结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>();
        /// <summary>
        /// 返回当前dynamic类型结果
        /// </summary>
        /// <returns></returns>
        object Get();
        /// <summary>
        /// 异步返回当前dynamic类型结果
        /// </summary>
        /// <returns></returns>
        Task<object> GetAsync();
        /// <summary>
        /// 返回当前T类型结果
        /// </summary>
        /// <typeparam name="T">结果集类型</typeparam>
        /// <returns></returns>
        T Get<T>();
        /// <summary>
        /// 异步返回当前T类型结果
        /// </summary>
        /// <typeparam name="T">结果集类型</typeparam>
        /// <returns></returns>
        Task<T> GetAsync<T>();
    }
}
