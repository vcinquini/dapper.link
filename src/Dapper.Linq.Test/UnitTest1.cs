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
            Assert.Equal(1, student.AuthorId);
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
                .Where(a => a.AuthorId > 1)
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
                GlobalSettings.XmlCommandsProvider.Load(@"C:\Users\vrcn\source\repos\dapper.link\src\Dapper.Linq.Test\authors.xml");

                using ( var multi = db.From("authors.list", new { author_id = 1, first_name = "Valmir" }).MultipleQuery() )
                {
                    var list2 = multi.GetList<Authors>();
                    var count = multi.Get<int>();
                    var mylist = db.Query("select * from authors");
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception Ex)
            {
                throw;
            }
#pragma warning restore CS0168 // Variable is declared but never used
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
                    AuthorId = 8,
                    FirstName = "Luiza",
                    LastName = "Pedro",
                    Phone = "+5511993218537"
                });

                //throw new Exception("rollback");
                db.From<Authors>().Insert(new Authors()
                {
                    AuthorId = 8,
                    FirstName = "Dirce",
                    LastName = "Pedro",
                    Phone = "+5511993218537"
                });

                db?.RollbackTransaction();
            });

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

            db.Open();

            db.BeginTransaction(IsolationLevel.ReadCommitted);

            db.From<Authors>().Delete(x => x.AuthorId == 9 || x.AuthorId == 10);

            db.From<Authors>().Insert(new Authors()
            {
                AuthorId = 9,
                FirstName = "Luiza",
                LastName = "Pedro",
                Phone = "+5511993218537"
            });

            db.From<Authors>().Insert(new Authors()
            {
                AuthorId = 10,
                FirstName = "Dirce",
                LastName = "Pedro",
                Phone = "+5511993218537"
            });

            db.CommitTransaction();

            var au9 = db.From<Authors>().Get(9);
            var au10 = db.From<Authors>().Get(10);

            Assert.True(au9.AuthorId == 9);
            Assert.True(au10.AuthorId == 10);

            db?.Close();

        }

        [Fact]
        public void Test11()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            try
            {
                //GlobalSettings.XmlCommandsProvider.Load(@"C:\Users\vrcn\source\repos\dapper.link\src\Dapper.Linq.Test\authors.xml");
                GlobalSettings.XmlCommandsProvider.LoadXml(@"
                    <commands namespace=""authors"">
                        <select id = ""list"">
                            <var id = ""where"">
                                <where >
                                    <if test = ""Id!=null"">
                                        AND author_id = @Id
                                    </if>
                                    <if test = ""Name!=null"">
                                        AND first_name = @Name
                                    </if>
                                    <if test = ""Name!=null"">
                                        AND last_name = @Name
                                    </if>
                                </where>
                            </var>
                            select * from authors ${where};
                            select count(1) from authors ${where}
                        </select>
                    </commands>");

                using (var multi = db
                                    .From("authors.list", new { author_id = 1, first_name = "Valmir", last_name = "Cinquini" } )
                                    .MultipleQuery())
                {
                    var list2 = multi.GetList<Authors>();
                    var count = multi.Get<int>();
                    var mylist = db.Query("select * from authors");
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception Ex)
            {
                throw;
            }
#pragma warning restore CS0168 // Variable is declared but never used
        }

    }
}