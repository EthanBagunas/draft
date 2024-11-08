using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string JobDepartment { get; set; }
        public string Address { get; set; }
        public string ContactNum { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
