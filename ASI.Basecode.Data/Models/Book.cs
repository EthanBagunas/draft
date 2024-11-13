using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public int? CustomerIdFk { get; set; }
        public int? RoomId { get; set; }
        public DateTime? BookingDate { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public int? Duration { get; set; }
        public string Status { get; set; }
        public DateTime? ReservationDate { get; set; }
    }
}
