using System;
using System.Collections.Generic;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Services.ServiceModels
{
    public class HomepageViewModel
    {
        public List<Room> Rooms { get; set; }
        public List<Book> Bookings { get; set; }
        public Dictionary<int, string> RoomStatuses { get; set; }

        public HomepageViewModel()
        {
            Rooms = new List<Room>();
            Bookings = new List<Book>();
            RoomStatuses = new Dictionary<int, string>();
        }
    }
}
