using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace FunctionAppTestDapper
{
    public class AuthorsRepository : IDisposable
    {

        DbContext _db;

        public AuthorsRepository() 
        {
            _db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection(Environment.GetEnvironmentVariable("DbConn")),
                DbContextType = DbContextType.SqlServer
            });

            _db.Open();
        }

        public void Dispose()
        {
            _db.Close();

        }

        public List<Author> GetAll()
        {
            return _db.From<Author>().Select().ToList();
        }

        public Author GetById(int Id)
        {
            return  _db.From<Author>().Get(Id);
        }
    }
}
