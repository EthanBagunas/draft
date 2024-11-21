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
        IEnumerable<Room> GetAllRooms();
        void AddRoom(RoomViewModel model);
        void DeleteRoom(int roomId);
        Dictionary<int, string> GetCurrentRoomStatuses();
        Room GetRoomById(int id);
        void UpdateRoom(EditRoomViewModel model);
    }
}
