using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper
{

    internal class DbMultipleResult : IDbMultipleResult
    {
        private readonly IDataReader _reader = null;

        private readonly IDbCommand _command = null;

        internal DbMultipleResult(IDbCommand command)
        {
            _command = command;
            _reader = command.ExecuteReader();
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _command?.Dispose();
        }

        public T Get<T>()
        {
            return GetList<T>().FirstOrDefault();
        }

        public async Task<T> GetAsync<T>()
        {
            return (await GetListAsync<T>()).FirstOrDefault();
        }

        public object Get()
        {
            return GetList<object>().FirstOrDefault();
        }

        public async Task<object> GetAsync()
        {
            return (await GetListAsync<object>()).FirstOrDefault();
        }

        public async Task<List<dynamic>> GetListAsync()
        {
            var handler = GlobalSettings.EntityMapperProvider.GetSerializer();
            var list = new List<dynamic>();
            while (await (_reader as DbDataReader).ReadAsync())
            {
                list.Add(handler(_reader));
            }
            NextResult();
            return list;
        }

        public List<dynamic> GetList()
        {
            var handler = GlobalSettings.EntityMapperProvider.GetSerializer();
            var list = new List<dynamic>();
            while (_reader.Read())
            {
                list.Add(handler(_reader));
            }
            NextResult();
            return list;
        }

        public List<T> GetList<T>()
        {
            var handler = GlobalSettings.EntityMapperProvider.GetSerializer<T>(_reader);
            var list = new List<T>();
            while (_reader.Read())
            {
                list.Add(handler(_reader));
            }
            NextResult();
            return list;
        }

        public async Task<List<T>> GetListAsync<T>()
        {
            var handler = GlobalSettings.EntityMapperProvider.GetSerializer<T>(_reader);
            var list = new List<T>();
            while (await (_reader as DbDataReader).ReadAsync())
            {
                list.Add(handler(_reader));
            }
            NextResult();
            return list;
        }

        public void NextResult()
        {
            if (!_reader.NextResult())
            {
                Dispose();
            }
        }
    }
}
