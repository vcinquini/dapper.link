using System;

namespace Dapper.Attributes
{
    /// <summary>
    /// Field mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        internal string Name { get; set; }
        /// <summary>
        /// attribute field mapping
        /// </summary>
        /// <param name="name">Database field name</param>
        public ColumnAttribute(string name = null)
        {
            Name = name;
        }
    }
}