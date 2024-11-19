using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Room
    {
        public int Id { get; set; }
        public string Roomname { get; set; }
        public int RoomNumber { get; set; }
        public int? MaxCapacity { get; set; }
        public string Status { get; set; }
    }
}
