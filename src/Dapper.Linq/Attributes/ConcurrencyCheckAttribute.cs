using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Attributes
{
    /// <summary>
    /// Concurrency check, if the field attribute is of type number, use timestamp, otherwise use GUID
    /// </summary>
    public class ConcurrencyCheckAttribute : Attribute
    {

    }
}
