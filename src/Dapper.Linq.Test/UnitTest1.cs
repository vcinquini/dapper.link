using System.Data;
using System.Data.SqlClient;

namespace Dapper.Linq.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            var student = db.From<Authors>().Get(1);

            Assert.NotNull(student);
            Assert.Equal(1, student.author_id);
        }

        [Fact]
        public void Test2()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            var list = db.From<Authors>()
                .Where(a => a.author_id > 1)
                .Select();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Fact]
        public void Test3()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            try
            {
                GlobalSettings.XmlCommandsProvider.Load(@"C:\Users\vcinq\source\dapper\Dapper.Linq\src\Dapper.Linq.Test\authors.xml");
                
                using ( var multi = db.From("authors.list", new { author_id = 1, first_name = "Valmir" }).MultipleQuery() )
                {
                    var list2 = multi.GetList<Authors>();
                    var count = multi.Get<int>();
                    var mylist = db.Query("select * from authors");
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        [Fact]
        public void Test4()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            var param = new SqlParameter("@Param1", 1);

            var list = db.Query<Authors>("GetById", param, 30, CommandType.StoredProcedure);

            Assert.NotNull(list);
            Assert.NotEmpty(list);

        }

        [Fact]
        public void Test5()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            Assert.Throws<SqlException>(() => {

                db.Open();

                db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                db.From<Authors>().Insert(new Authors()
                {
                    author_id = 8,
                    first_name = "Luiza",
                    last_name = "Pedro",
                    phone = "+5511993218537"
                });

                //throw new Exception("rollback");
                db.From<Authors>().Insert(new Authors()
                {
                    author_id = 8,
                    first_name = "Dirce",
                    last_name = "Pedro",
                    phone = "+5511993218537"
                });

                db?.RollbackTransaction();
            });

            //db.CommitTransaction();

            db?.Close();

        }

        [Fact]
        public void Test6()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            Assert. Throws<SqlException>(() => {

                db.Open();

                db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                db.From<Authors>().Delete(x => x.author_id == 9 || x.author_id == 10);

                db.From<Authors>().Insert(new Authors()
                {
                    author_id = 9,
                    first_name = "Luiza",
                    last_name = "Pedro",
                    phone = "+5511993218537"
                });

                db.From<Authors>().Insert(new Authors()
                {
                    author_id = 10,
                    first_name = "Dirce",
                    last_name = "Pedro",
                    phone = "+5511993218537"
                });

                //db?.RollbackTransaction();
                db.CommitTransaction();
            });


            db?.Close();

        }

    }
}