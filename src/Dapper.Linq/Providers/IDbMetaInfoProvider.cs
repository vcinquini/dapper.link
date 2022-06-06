using System;
using System.Collections.Generic;

namespace Dapper
{
    /// <summary>
    /// Database metadata provider
    /// </summary>
    public interface IDbMetaInfoProvider
    {
        /// <summary>
        /// Get the meta information of the table
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DbTableMetaInfo GetTable(Type type);
        /// <summary>
        /// Get the meta information of the field
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<DbColumnMetaInfo> GetColumns(Type type);
    }
}