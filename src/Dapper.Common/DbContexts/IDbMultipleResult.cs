using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper
{
    public interface IDbMultipleResult : IDisposable
    {
        List<dynamic> GetList();
        Task<List<dynamic>> GetListAsync();
        List<T> GetList<T>();
        Task<List<T>> GetListAsync<T>();
        object Get();
        Task<object> GetAsync();
        T Get<T>();
        Task<T> GetAsync<T>();
    }
}
