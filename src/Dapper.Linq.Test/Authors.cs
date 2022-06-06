using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Attributes;

namespace Dapper.Linq.Test
{
    public class Authors
    {
        [Key]
        [PrimaryKey]
        public int author_id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string phone { get; set; }
    }
}
