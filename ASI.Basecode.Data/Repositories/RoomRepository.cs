using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class RoomRepository: BaseRepository, IRoomRepository
    {
        public RoomRepository(IUnitOfWork unitOfWork): base(unitOfWork) { }

        public void  AddRoom(Room room)
        {
            try
            {
                Console.WriteLine($"Adding room: Name={room.Roomname}, Number={room.RoomNumber}");
                this.GetDbSet<Room>().Add(room);
                UnitOfWork.SaveChanges();
                Console.WriteLine("Room added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding room: {ex.Message}");
                throw;
            }
        }
        public void UpdateRoom(Room room)

        {
            this.GetDbSet<Room>().Update(room);
            UnitOfWork.SaveChanges();

        }
        public IEnumerable<Room> GetAll()
        {
            try
            {
                Console.WriteLine("Fetching all rooms");
                var rooms = this.GetDbSet<Room>().ToList();
                Console.WriteLine($"Found {rooms.Count} rooms");
                return rooms;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rooms: {ex.Message}");
                throw;
            }
        }
    }
}
