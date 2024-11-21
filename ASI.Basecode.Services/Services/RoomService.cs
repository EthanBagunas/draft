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
        private readonly IBookRepository _bookRepository;
        public RoomService(IRoomRepository repository, IMapper mapper, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _bookRepository = bookRepository;
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
                
                _repository.AddRoom(room);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Failed to add room: {ex.Message}", ex);
            }
        }
        public void UpdateRoom(EditRoomViewModel model)
        {
            var room =  _repository.GetAll().FirstOrDefault(r => r.Id ==model.Roomid);
            try
            {
                _mapper.Map(model, room);
                _repository.UpdateRoom(room);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update room: {ex.Message}", ex);
            }
        }
        public void DeleteRoom(int roomId)
        {
            try
            {
                // Get the room to be deleted
                var room = _repository.GetAll().FirstOrDefault(r => r.Id == roomId);
                if (room != null && room.RoomNumber.HasValue)
                {
                    // Store the current room number
                    var deletedRoomNumber = room.RoomNumber.Value;

                    // Set the room as inactive and null its room number
                    room.Status = "INACTIVE";
                    room.RoomNumber = null;
                    _repository.UpdateRoom(room);

                    // Update the room numbers for all rooms that come after the deleted room
                    var roomsToUpdate = _repository.GetAll()
                        .Where(r => r.Status != "INACTIVE" && r.RoomNumber > deletedRoomNumber)
                        .OrderBy(r => r.RoomNumber)
                        .ToList();

                    foreach (var roomToUpdate in roomsToUpdate)
                    {
                        if (roomToUpdate.RoomNumber.HasValue)
                        {
                            roomToUpdate.RoomNumber = roomToUpdate.RoomNumber.Value - 1;
                            _repository.UpdateRoom(roomToUpdate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete room: {ex.Message}", ex);
            }
        }
        
        public IEnumerable<Room> GetAllRooms()
        {
            return _repository.GetAll().Where(r => r.Status != "INACTIVE").ToList();
        }

        public Room GetRoomById(int id)
        {
            return _repository.GetAll().FirstOrDefault(r => r.Id == id);
        }

        public Dictionary<int, string> GetCurrentRoomStatuses()
        {
            var currentTime = DateTime.Now;
            var statuses = new Dictionary<int, string>();
            var rooms = GetAllRooms();

            foreach (var room in rooms)
            {
                // Get all bookings for today that are not cancelled or completed
                var todayBookings = _bookRepository.GetAllBooks()
                    .Where(b => b.RoomId == room.Id && 
                           b.BookingDate.HasValue && 
                           b.BookingDate.Value.Date == currentTime.Date &&
                           b.Status != "CANCELLED" &&
                           b.Status != "COMPLETED")
                    .OrderBy(b => b.TimeIn)
                    .ToList();

                if (!todayBookings.Any())
                {
                    statuses[room.Id] = "Vacant";
                    continue;
                }

                // Check if there's a current booking (time is between TimeIn and TimeOut)
                var currentBooking = todayBookings
                    .FirstOrDefault(b => 
                        currentTime.TimeOfDay >= b.TimeIn && 
                        currentTime.TimeOfDay <= b.TimeOut);

                if (currentBooking != null)
                {
                    statuses[room.Id] = "Occupied";
                }
                // Check if there's an upcoming booking today
                else if (todayBookings.Any(b => currentTime.TimeOfDay < b.TimeIn))
                {
                    statuses[room.Id] = "Reserved";
                }
                else
                {
                    statuses[room.Id] = "Vacant";
                }
            }

            return statuses;
        }
    }
    

   
}
