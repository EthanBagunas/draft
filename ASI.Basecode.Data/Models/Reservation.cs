using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Reservation
    {
        public int ReservationId { get; set; }
        public int? CustomerIdFk { get; set; }
        public int? RoomId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public DateTime? DateRange { get; set; }
    }
}
