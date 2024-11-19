using System;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public TimeSpan TimeIn { get; set; }
        [Required]
        public TimeSpan TimeOut { get; set; }
        [Required]
        public string GuestName { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        public int? CustomerId { get; set; }
        public int? Duration { get; set; }
        public string Status { get; set; }
        public DateTime? ReservationDate { get; set; }
    }
}
