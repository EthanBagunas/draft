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
            var room = new Room();
            try 
            {
                _mapper.Map(model, room);
                _repository.AddRoom(room);
            }
            catch (Exception ex) 
            {
               Console.WriteLine(ex.Message);
            }
        }
        public void UpdateRoom(RoomViewModel model)
        {

        }
        
        public IEnumerable<Room> GetAllRooms()
        {
            // Replace this with your actual data retrieval logic

            
            return _repository.GetAll().ToList();
        }
    }
    

   
}
