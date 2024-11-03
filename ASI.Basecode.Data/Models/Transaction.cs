using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? CustomerIdFk { get; set; }
        public int? EmployeeIdFk { get; set; }
        public int? ReservationIdFk { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
