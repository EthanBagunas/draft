using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASI.Basecode.Data.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        
        [Column("customer_ID_FK")]
        public int? CustomerId { get; set; }
        
        [Column("room_ID")]
        public int? RoomId { get; set; }
        
        [Column("booking_date")]
        public DateTime? BookingDate { get; set; }
        
        [Column("time_in")]
        public TimeSpan? TimeIn { get; set; }
        
        [Column("time_out")]
        public TimeSpan? TimeOut { get; set; }
        
        [Column("duration")]
        public int? Duration { get; set; }
        
        [Column("status")]
        [StringLength(10)]
        public string Status { get; set; }
        
        [Column("reservation_date")]
        public DateTime? ReservationDate { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
    }
}
