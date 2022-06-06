using Dapper.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Dapper
{

    /// <summary>
    /// Annotation scheme database metadata
    /// </summary>
    public class AnnotationDbMetaInfoProvider : IDbMetaInfoProvider
    {
        private static readonly ConcurrentDictionary<Type, DbTableMetaInfo> _tables
            = new ConcurrentDictionary<Type, DbTableMetaInfo>();

        private static readonly ConcurrentDictionary<Type, List<DbColumnMetaInfo>> _columns
            = new ConcurrentDictionary<Type, List<DbColumnMetaInfo>>();

        public DbTableMetaInfo GetTable(Type type)
        {
            return _tables.GetOrAdd(type, t =>
            {
                var name = t.Name;
                if (t.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() != null)
                {
                    var attribute = t.GetCustomAttributes(typeof(TableAttribute), true)
                        .FirstOrDefault() as TableAttribute;
                    name = attribute.Name;
                }
                var table = new DbTableMetaInfo()
                {
                    TableName = name,
                    CsharpName = t.Name
                };
                return table;
            });
        }

        public List<DbColumnMetaInfo> GetColumns(Type type)
        {
            return _columns.GetOrAdd(type, t =>
            {
                var list = new List<DbColumnMetaInfo>();
                var properties = type.GetProperties();

                foreach (var item in properties)
                {
                    var columnName = item.Name;
                    var isPrimaryKey = false;
                    var isDefault = false;
                    var isIdentity = false;
                    var isNotMapped = false;
                    var isConcurrencyCheck = false;
                    var isComplexType = false;

                    if (item.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() != null)
                    {
                        var attribute = item.GetCustomAttributes(typeof(ColumnAttribute), true)
                            .FirstOrDefault() as ColumnAttribute;
                        columnName = attribute.Name;
                    }
                    if (item.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).FirstOrDefault() != null)
                    {
                        isPrimaryKey = true;
                    }
                    if (item.GetCustomAttributes(typeof(IdentityAttribute), true).FirstOrDefault() != null)
                    {
                        isIdentity = true;
                    }
                    if (item.GetCustomAttributes(typeof(DefaultAttribute), true).FirstOrDefault() != null)
                    {
                        isDefault = true;
                    }
                    if (item.GetCustomAttributes(typeof(ConcurrencyCheckAttribute), true).FirstOrDefault() != null)
                    {
                        isConcurrencyCheck = true;
                    }
                    if (item.GetCustomAttributes(typeof(NotMappedAttribute), true).FirstOrDefault() != null)
                    {
                        isNotMapped = true;
                    }
                    if (item.GetCustomAttributes(typeof(ComplexTypeAttribute), true).FirstOrDefault() != null)
                    {
                        isComplexType = true;
                    }
                    list.Add(new DbColumnMetaInfo()
                    {
                        CsharpType = item.PropertyType,
                        IsDefault = isDefault,
                        ColumnName = columnName,
                        CsharpName = item.Name,
                        IsPrimaryKey = isPrimaryKey,
                        IsIdentity = isIdentity,
                        IsNotMapped = isNotMapped,
                        IsConcurrencyCheck = isConcurrencyCheck,
                        IsComplexType = isComplexType
                    });
                }
                return list;
            });
        }

    }

    /// <summary>
    /// table information
    /// </summary>
    public class DbTableMetaInfo
    {
        /// <summary>
        /// database table name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Csharp table name
        /// </summary>
        public string CsharpName { get; set; }
    }

    /// <summary>
    /// Field information
    /// </summary>
    public class DbColumnMetaInfo
    {
        /// <summary>
        /// Check for concurrency
        /// </summary>
        public bool IsConcurrencyCheck { get; set; }

        /// <summary>
        /// Whether the default value constraint
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Is it a database field
        /// </summary>
        public bool IsNotMapped { get; set; }

        /// <summary>
        /// database field name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Csharp field name
        /// </summary>
        public string CsharpName { get; set; }

        /// <summary>
        /// Csharp type
        /// </summary>
        public Type CsharpType { get; set; }

        /// <summary>
        /// primary key constraint
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Whether it is an auto-incrementing column
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Whether it is a computed column
        /// </summary>
        public bool IsComplexType { get; set; }
    }
}