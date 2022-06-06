using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// database context
    /// </summary>
    public class DbContext : IDbContext
    {
        private IDbTransaction _transaction = null;

        public event Logging Logging;
        public DbContextState DbContextState = DbContextState.Closed;
        
        public IDbConnection Connection { get; } = null;
        public DbContextType DbContextType { get; } = DbContextType.Mysql;

        public DbContext(DbContextBuilder builder)
        {
            Connection = builder.Connection;
            DbContextType = builder.DbContextType;
        }
        public IXmlQuery From<T>(string id, T parameter) where T : class
        {
            var sql = GlobalSettings.XmlCommandsProvider.Build(id, parameter);
            var deserializer = GlobalSettings.EntityMapperProvider.GetDeserializer(typeof(T));
            var values = deserializer(parameter);
            return new XmlQuery(this, sql, values);
        }

        public IXmlQuery From(string id)
        {
            var sql = GlobalSettings.XmlCommandsProvider.Build(id);
            return new XmlQuery(this, sql);
        }

        public IDbQuery<T> From<T>()
        {
            return new DbQuery<T>(this);
        }

        public IEnumerable<dynamic> Query(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = Connection.CreateCommand())
            {
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                var list = new List<dynamic>();
                using (var reader = cmd.ExecuteReader())
                {
                    var handler = GlobalSettings.EntityMapperProvider.GetSerializer();
                    while (reader.Read())
                    {
                        list.Add(handler(reader));
                    }
                    return list;
                }
            }
        }
        public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = (Connection as DbConnection).CreateCommand())
            {
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<dynamic>();
                    var handler = GlobalSettings.EntityMapperProvider.GetSerializer();
                    while (reader.Read())
                    {
                        list.Add(handler(reader));
                    }
                    return list;
                }
            }
        }
        public IDbMultipleResult QueryMultiple(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var cmd = Connection.CreateCommand();
            Initialize(cmd, sql, parameter, commandTimeout, commandType);
            return new DbMultipleResult(cmd);
        }
        public IEnumerable<T> Query<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = Connection.CreateCommand())
            {
                var list = new List<T>();
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                using (var reader = cmd.ExecuteReader())
                {
                    var handler = GlobalSettings.EntityMapperProvider.GetSerializer<T>(reader);
                    while (reader.Read())
                    {
                        list.Add(handler(reader));
                    }
                    return list;
                }
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = (Connection as DbConnection).CreateCommand())
            {
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new List<T>();
                    var handler = GlobalSettings.EntityMapperProvider.GetSerializer<T>(reader);
                    while (await reader.ReadAsync())
                    {
                        list.Add(handler(reader));
                    }
                    return list;
                }
            }
        }
        public int Execute(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = Connection.CreateCommand())
            {
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                return cmd.ExecuteNonQuery();
            }
        }
        public async Task<int> ExecuteAsync(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = (Connection as DbConnection).CreateCommand())
            {
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                return await cmd.ExecuteNonQueryAsync();
            }
        }
        public T ExecuteScalar<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = Connection.CreateCommand())
            {
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                var result = cmd.ExecuteScalar();
                if (result is DBNull || result == null)
                {
                    return default;
                }
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameter = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var cmd = (Connection as DbConnection).CreateCommand())
            {
                Initialize(cmd, sql, parameter, commandTimeout, commandType);
                var result = await cmd.ExecuteScalarAsync();
                if (result is DBNull || result == null)
                {
                    return default;
                }
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
        public async Task BeginTransactionAsync()
        {
            await Task.Run(() =>
            {
                _transaction = Connection.BeginTransaction();
                Logging?.Invoke("Begin Transaction");
            });
        }
        public async Task BeginTransactionAsync(IsolationLevel level)
        {
            await Task.Run(() =>
            {
                _transaction = Connection.BeginTransaction(level);
                Logging?.Invoke("Begin Transaction IsolationLevel = " + level);
            });
        }
        public void BeginTransaction()
        {
            _transaction = Connection.BeginTransaction();
            Logging?.Invoke("Begin Transaction");
        }
        public void BeginTransaction(IsolationLevel level)
        {
            _transaction = Connection.BeginTransaction(level);
            Logging?.Invoke("Begin Transaction IsolationLevel = " + level);
        }
        public void Close()
        {
            _transaction?.Dispose();
            Connection?.Close();
            DbContextState = DbContextState.Closed;
            Logging?.Invoke("Colsed Connection");
        }
        public void CommitTransaction()
        {
            _transaction?.Commit();
            DbContextState = DbContextState.Commit;
            Logging?.Invoke("Commit Transaction");
        }
        public void Open()
        {
            Connection?.Open();
            DbContextState = DbContextState.Open;
            Logging?.Invoke("Open Connection");
        }
        public async Task OpenAsync()
        {
            await (Connection as DbConnection).OpenAsync();
            DbContextState = DbContextState.Open;
            Logging?.Invoke("Open Connection");
        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            DbContextState = DbContextState.Rollback;
            Logging?.Invoke("Rollback");
        }
        private void Initialize(IDbCommand cmd, string sql, object parameter, int? commandTimeout = null, CommandType? commandType = null)
        {
            var dbParameters = new List<IDbDataParameter>();
            cmd.Transaction = _transaction;
            cmd.CommandText = sql;

            if (commandTimeout.HasValue)
            {
                cmd.CommandTimeout = commandTimeout.Value;
            }
            if (commandType.HasValue)
            {
                cmd.CommandType = commandType.Value;
            }
            if (parameter is IDbDataParameter)
            {
                dbParameters.Add(parameter as IDbDataParameter);
            }
            else if (parameter is IEnumerable<IDbDataParameter> parameters)
            {
                dbParameters.AddRange(parameters);
            }
            else if (parameter is Dictionary<string, object> keyValues)
            {
                foreach (var item in keyValues)
                {
                    var param = CreateParameter(cmd, item.Key, item.Value);
                    dbParameters.Add(param);
                }
            }
            else if (parameter != null)
            {
                var handler = GlobalSettings.EntityMapperProvider.GetDeserializer(parameter.GetType());
                var values = handler(parameter);
                foreach (var item in values)
                {
                    var param = CreateParameter(cmd, item.Key, item.Value);
                    dbParameters.Add(param);
                }
            }
            if (dbParameters.Count > 0)
            {
                foreach (IDataParameter item in dbParameters)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                    var pattern = $@"in\s+([\@,\:,\?]?{item.ParameterName})";
                    var options = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline;
                    if (cmd.CommandText.IndexOf("in", StringComparison.OrdinalIgnoreCase) != -1 && Regex.IsMatch(cmd.CommandText, pattern, options))
                    {
                        var name = Regex.Match(cmd.CommandText, pattern, options).Groups[1].Value;
                        var list = new List<object>();
                        if (item.Value is IEnumerable<object> || item.Value is Array)
                        {
                            list = (item.Value as IEnumerable).Cast<object>().Where(a => a != null && a != DBNull.Value).ToList();
                        }
                        else
                        {
                            list.Add(item.Value);
                        }
                        if (list.Count() > 0)
                        {
                            cmd.CommandText = Regex.Replace(cmd.CommandText, name, $"({string.Join(",", list.Select(s => $"{name}{list.IndexOf(s)}"))})");
                            foreach (var iitem in list)
                            {
                                var key = $"{item.ParameterName}{list.IndexOf(iitem)}";
                                var param = CreateParameter(cmd, key, iitem);
                                cmd.Parameters.Add(param);
                            }
                        }
                        else
                        {
                            cmd.CommandText = Regex.Replace(cmd.CommandText, name, $"(SELECT 1 WHERE 1 = 0)");
                        }
                    }
                    else
                    {
                        cmd.Parameters.Add(item);
                    }
                }
            }
            if (Logging != null)
            {
                var parameters = new Dictionary<string, object>();
                foreach (IDbDataParameter item in cmd.Parameters)
                {
                    parameters.Add(item.ParameterName, item.Value);
                }
                Logging.Invoke(cmd.CommandText, parameters, commandTimeout, commandType);
            }
        }
        private IDbDataParameter CreateParameter(IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            Connection?.Dispose();
        }
    }

    /// <summary>
    /// sql execution log
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    public delegate void Logging(string message, Dictionary<string, object> parameters = null, int? commandTimeout = null, CommandType? commandType = null);
}
