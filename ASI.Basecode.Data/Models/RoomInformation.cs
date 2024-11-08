using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class RoomInformation
    {
        public int Id { get; set; }
        public string AvailableSlots { get; set; }
        public string Price { get; set; }
    }
}
