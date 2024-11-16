using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;


namespace ASI.Basecode.Services.Services
{
    public  class BookService: IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository repository, IMapper mapper ) 
        {
            _mapper = mapper;
            _repository = repository;
        }

        public void AddBook(BookViewModel model)
        {
           var book = new Book();
            try 
            {
                _mapper.Map(model, book);
                book.ReservationDate= DateTime.Now;
                book.Duration = (int)(model.TimeOut.Value - model.TimeIn.Value).TotalHours;
                _repository.CreateBook(book);
            }

            catch (Exception ex)
            {
                Console.WriteLine("An unexpected exception occurred: " + ex.Message);
            }

        }
        public IEnumerable<Book> GetAllBooksbyId(int roomid)
        {
            return _repository.GetAllBooks().Where(x=> x.RoomId == roomid).ToList(); ;
        }

        /*public async Task<Room> GetRoomWithBookingsAsync(int roomId)

        {

            return await _repository.GetRoomWithBookingsAsync(roomId);
        }
        */
        public void UpdateBook(BookViewModel model)
        {
            /*var book = new Book();
            try
            {
                _mapper.Map(model, Book);
                _repository.UpdateBook(Book);
            }

            catch (Exception ex)
            {
                Console.WriteLine("An unexpected exception occurred: " + ex.Message);
            }*/
        }            
    }
}
