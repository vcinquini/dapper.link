using System;

namespace Dapper.Attributes
{
    /// <summary>
    /// ignore mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMappedAttribute: Attribute
    {

    }
}
