using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASI.Basecode.Data.Models
{
    public partial class Room
    {
        public int Id { get; set; }
        [Column("roomname")]
        public string Roomname { get; set; }
        [Column("max_capacity")]
        public int? MaxCapacity { get; set; }
        public string Status { get; set; }
        public int RoomNumber { get; set; }
    }
}
