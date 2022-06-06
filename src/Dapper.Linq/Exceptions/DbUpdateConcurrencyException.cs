using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Expressions
{
    /// <summary>
    /// Concurrency conflict when modifying data
    /// </summary>
    public class DbUpdateConcurrencyException : Exception
    {
        public DbUpdateConcurrencyException(string message)
            : base(message)
        {

        }
    }
}
