using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;


namespace ASI.Basecode.Services.Services
{
    public class RoomService: IRoomService
    {
        private readonly IRoomRepository _repository;
        private readonly IMapper _mapper;
        public RoomService(IRoomRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public void AddRoom(RoomViewModel model)
        {
            if (string.IsNullOrEmpty(model.Roomname))
                throw new ArgumentException("Room name cannot be empty");

            if (!int.TryParse(model.MaxCapacity, out int capacity))
                throw new ArgumentException("Invalid capacity value");

            var room = new Room();
            try 
            {
                var maxRoomNumber = _repository.GetAll()
                    .Select(r => r.RoomNumber)
                    .DefaultIfEmpty(0)
                    .Max();

                room.RoomNumber = maxRoomNumber + 1;
                room.Roomname = model.Roomname;
                room.MaxCapacity = capacity;
                room.Status = "ACTIVE";
                
                Console.WriteLine($"Attempting to add room: {room.Roomname} with number {room.RoomNumber}");
                _repository.AddRoom(room);
                Console.WriteLine("Room added successfully");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Failed to add room: {ex.Message}");
                throw new Exception($"Failed to add room: {ex.Message}", ex);
            }
        }
        public void UpdateRoom(RoomViewModel model)
        {
            // Implementation for update
        }
        public void DeleteRoom(int roomId)
        {
            try
            {
                var room = _repository.GetAll().FirstOrDefault(r => r.Id == roomId);
                if (room != null)
                {
                    var deletedRoomNumber = room.RoomNumber;
                    
                    // Delete the room
                    _repository.UpdateRoom(room); // Set status to inactive or delete

                    // Reorder remaining room numbers
                    var roomsToUpdate = _repository.GetAll()
                        .Where(r => r.RoomNumber > deletedRoomNumber)
                        .OrderBy(r => r.RoomNumber)
                        .ToList();

                    foreach (var r in roomsToUpdate)
                    {
                        r.RoomNumber--;
                        _repository.UpdateRoom(r);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public IEnumerable<Room> GetAllRooms()
        {
            return _repository.GetAll().ToList();
        }

       
    }
    

   
}
