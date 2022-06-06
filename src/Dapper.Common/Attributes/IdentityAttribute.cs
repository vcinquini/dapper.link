using System;

namespace Dapper.Attributes
{
    /// <summary>
    /// auto-incrementing column ID
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute
    {

    }
}
