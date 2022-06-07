using System.Data;
using System.Data.SqlClient;


namespace Dapper.Linq.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Get()
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
        public void Test_Query_Linq()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var list = db.From<Authors>()
                .Where(a => a.AuthorId > 1 && !a.LastName.Contains("Cinquini"))
                .Select();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Fact]
        public void Test_Multiple_Xml_File()
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
        public void Test_Procedure()
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
        public void Test_Query_String()
        {
            int id = 2;

            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            var list = db.Query<Authors>($"SELECT * FROM authors where author_id = {id}", commandType: CommandType.Text);

            Assert.NotNull(list);
            Assert.NotEmpty(list);

        }

        [Fact]
        public void Test_Insert_Rollback()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            Assert.Throws<SqlException>(() => {

                db.Open();

                db.BeginTransaction(IsolationLevel.ReadCommitted);

                db.From<Authors>().Insert(new Authors()
                {
                    AuthorId = 11,
                    FirstName = "Luiza",
                    LastName = "Pedro",
                    Phone = "+5511993218537"
                });

                //throw new Exception("rollback");
                db.From<Authors>().Insert(new Authors()
                {
                    AuthorId = 11,
                    FirstName = "Dirce",
                    LastName = "Pedro",
                    Phone = "+5511993218537"
                });

                db?.RollbackTransaction();
            });

            db?.Close();

        }

        [Fact]
        public void Test_Insert_Commit()
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
        public void Test_Insert_Commit_2()
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
                AuthorId = 90,
                FirstName = "Luiza",
                LastName = "Pedro",
                Phone = "+5511993218537"
            });

            db.From<Authors>().Insert(new Authors()
            {
                AuthorId = 100,
                FirstName = "Dirce",
                LastName = "Pedro",
                Phone = "+5511993218537"
            });

            db.CommitTransaction();

            var au9 = db.From<Authors>().Get(90);
            var au10 = db.From<Authors>().Get(100);

            Assert.True(au9.AuthorId == 90);
            Assert.True(au10.AuthorId == 100);

            db?.Close();

        }
        [Fact]
        public void Test_Multiple_Xml_Text()
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

        [Fact]
        public void Test_Update()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            ////////////////////////////
            var age = "20";

            var row1 = db.From<Authors>()
                .Set(a => a.FirstName, "Valmir_")
                .Set(a => a.Phone, "")
                .Set(a => a.LastName, a => a.LastName + age)
                .Where(a => a.AuthorId == 100)
                .Update();

            var row2 = db.From<Authors>()
                .Set(a => a.FirstName, "Valmir ")
                .Set(a => a.Phone, "")
                .Set(a => a.LastName, a => a.LastName + age)
                .Where(a => Operator.In(a.AuthorId, 100, 102))
                .Update();


            //entity update by primary key
            var row3 = db.From<Authors>()
                .Filter(a => a.AuthorId)
                .Update(new Authors()
                {
                    AuthorId = 2,
                    FirstName = "Isadora",
                    LastName = "M. R. Cinquini",
                    Phone = "+5511999701810"
                });
            ////////////////////////////

        }

        [Fact]
        public void Test_Page()
        {
            var db = new DbContext(new DbContextBuilder
            {
                Connection = new SqlConnection("server=valmirsqlserver.database.windows.net;user id=vcinquini;password=V@lmir01;database=ValmirFunctionDB;"),
                DbContextType = DbContextType.SqlServer
            });

            db.Open();

            var total = db.From<Authors>()
                .Page(1, 10)
                .Select();

            Assert.Equal(10, total.Last<Authors>().AuthorId);
            Assert.Equal(10, total.Count());

            total = db.From<Authors>()
                .Page(10, 2)
                .Select();

            Assert.Equal(102, total.Last<Authors>().AuthorId);
            Assert.Equal(4, total.Count());

        }
    }
}

/*

    //lock
    var student = db.From<Authors>()
        .Where(a => a.AuthorId == 10)
        .Single();

*/