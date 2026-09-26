using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SchoolLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }

        public string SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string UserName { get; set; }

        public int TotalRowCount { get; set; }
    }
}
