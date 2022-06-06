using System;

namespace Dapper.Attributes
{
    /// <summary>
    /// table name mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        internal string Name { get; set; }
        public TableAttribute(string name = null)
        {
            Name = name;
        }
    }
}
