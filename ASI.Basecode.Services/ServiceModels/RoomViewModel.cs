using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
        public class RoomViewModel
        {
            public string Roomname { get; set; }
            public string MaxCapacity { get; set; }
        }
        public class EditRoomViewModel : RoomViewModel
        {
            public int Roomid { get; set; }
        }
}
