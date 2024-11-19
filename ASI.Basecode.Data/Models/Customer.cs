using System;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Data.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string Custfname { get; set; }
        public string Status { get; set; }
        public string Contact { get; set; }
    }
}
