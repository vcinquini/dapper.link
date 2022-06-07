using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Attributes;

namespace FunctionAppTestDapper
{
    [Table("Authors")]
    public class Author
    {
        [PrimaryKey]
        [Column("author_id")]
        public int AuthorId { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("phone")]
        public string Phone { get; set; }
    }
}
