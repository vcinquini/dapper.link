using System;
using System.Collections.Generic;
using System.Data;

namespace Dapper
{
    /// <summary>
    /// Entity conversion mapper
    /// </summary>
    public interface IEntityMapperProvider
    {
        /// <summary>
        /// Get the entity serializer converter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        /// <returns></returns>
        Func<IDataRecord, T> GetSerializer<T>(IDataRecord record);
        /// <summary>
        /// Get dynamic entity serialization converter
        /// </summary>
        /// <returns></returns>
        Func<IDataRecord, dynamic> GetSerializer();
        /// <summary>
        /// Get the parameter decoder
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Func<object, Dictionary<string, object>> GetDeserializer(Type type);
    }

}