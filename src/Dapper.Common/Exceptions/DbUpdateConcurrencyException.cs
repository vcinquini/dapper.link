using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Expressions
{
    public class DbUpdateConcurrencyException : Exception
    {
        public DbUpdateConcurrencyException(string message)
            : base(message)
        {

        }
    }
}
