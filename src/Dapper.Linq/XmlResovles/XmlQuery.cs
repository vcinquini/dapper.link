using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// Implement the xml command mapper
    /// </summary>
    internal class XmlQuery : IXmlQuery
    {
        private readonly string _sql = null;

        private Dictionary<string, object> _parameters = null;

        private readonly IDbContext _mapper = null;

        public XmlQuery(IDbContext mapper, string sql, Dictionary<string,object> parameters=null)
        {
            _mapper = mapper;
            _sql = sql;
            _parameters = parameters;
        }

        public IDbMultipleResult MultipleQuery(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.QueryMultiple(_sql, _parameters, commandTimeout,commandType);
        }

        public int Execute(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.Execute(_sql, _parameters, commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.ExecuteAsync(_sql, _parameters, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.Query<T>(_sql, _parameters, commandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.Query(_sql, _parameters, commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.QueryAsync<T>(_sql, _parameters, commandTimeout, commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.QueryAsync(_sql, _parameters, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.ExecuteScalar<T>(_sql, _parameters, commandTimeout, commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(int? commandTimeout = null, CommandType? commandType = null)
        {
            return _mapper.ExecuteScalarAsync<T>(_sql, _parameters, commandTimeout, commandType);
        }

        public void AddDbParameter(string name,object value)
        {
            if (_parameters == null)
            {
                _parameters = new Dictionary<string, object>();
            }
            if (_parameters.ContainsKey(name))
            {
                _parameters[name] = value;
            }
            else
            {
                _parameters.Add(name,value);
            }
        }
    }
}
