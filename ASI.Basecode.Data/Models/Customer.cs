using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Customer
    {
        public int CustId { get; set; }
        public string Custfname { get; set; }
        public string Custlname { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}
