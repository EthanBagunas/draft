using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IRoomService
    {
        void AddRoom(RoomViewModel model);
        void UpdateRoom(RoomViewModel model);
        IEnumerable<Room> GetAllRooms();

    }
}
