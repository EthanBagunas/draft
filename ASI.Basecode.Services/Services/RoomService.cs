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
                    room.Status = "INACTIVE";
                    _repository.UpdateRoom(room);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete room: {ex.Message}", ex);
            }
        }
        
        public IEnumerable<Room> GetAllRooms()
        {
            return _repository.GetAll().ToList();
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
                var currentBooking = _bookRepository.GetAllBooks()
                    .Where(b => b.RoomId == room.Id && 
                           b.BookingDate.HasValue && 
                           b.BookingDate.Value.Date == currentTime.Date)
                    .OrderBy(b => b.TimeIn)
                    .FirstOrDefault();

                if (currentBooking == null)
                {
                    statuses[room.Id] = "Vacant";
                    continue;
                }

                if (currentTime.TimeOfDay < currentBooking.TimeIn)
                    statuses[room.Id] = "Reserved";
                else if (currentTime.TimeOfDay >= currentBooking.TimeIn && 
                         currentTime.TimeOfDay <= currentBooking.TimeOut)
                    statuses[room.Id] = "Occupied";
                else
                    statuses[room.Id] = "Vacant";
            }

            return statuses;
        }
    }
    

   
}
