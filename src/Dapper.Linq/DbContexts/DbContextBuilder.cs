using Dapper.Expressions;
using System.Data;

namespace Dapper
{
    public class DbContextBuilder
    {
        /// <summary>
        /// Set up the database connection to be hosted
        /// </summary>
        public IDbConnection Connection { get; set; }
        /// <summary>
        /// set database type
        /// </summary>
        public DbContextType DbContextType { get; set; }
    }
}
