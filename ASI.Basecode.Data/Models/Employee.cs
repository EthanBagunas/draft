using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string JobDepartment { get; set; }
        public string Address { get; set; }
        public string ContactNum { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
