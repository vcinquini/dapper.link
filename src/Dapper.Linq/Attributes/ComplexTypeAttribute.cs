using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Attributes
{
    /// <summary>
    /// Calculated column, if one of the fields is a calculated column, it will not be processed when adding and modifying
    /// </summary>
    public class ComplexTypeAttribute : Attribute
    {

    }
}
