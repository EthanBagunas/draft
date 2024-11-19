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
using static ASI.Basecode.Resources.Constants.Enums;
using System.Timers;
using System.Threading.Tasks;
using ASI.Basecode.Data;
using CsvHelper.TypeConversion;

namespace ASI.Basecode.Services.Services
{
    public  class BookService: IBookService, ITimedBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private static Timer _timer;

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
        public void UpdateBook(int bookid)
        {
            var book = _repository.GetAllBooks().FirstOrDefault(x => x.Id == bookid);
            try
            {
                book.Status = "Completed";
                _repository.UpdateBook(book);
            }

            catch (Exception ex)
            {
                Console.WriteLine("An unexpected exception occurred: " + ex.Message);
            }
        }
        public void  UpdateCompletedBooks()
        {
            var current = DateTime.Now;
            TimeSpan currentTime = current.TimeOfDay;
            /*
            string datestring = current.Date.ToString("yyyy-MM-dd");
            DateTime date = DateTime.Parse(datestring);
            Console.WriteLine(datestring);
            Console.WriteLine(currentTime);
            */
            var books = _repository.GetAllBooks().Where(x => x.TimeOut < currentTime). ToList();
            Console.WriteLine(books);
            foreach (var book in books)
            {
                /// if book.BookingDate = current date
                if (currentTime > book.TimeOut)
                {
                    book.Status = "Completed";
                    _repository.UpdateBook(book);
                }
                else if (currentTime < book.TimeIn)
                {
                    book.Status = "Pending";
                    _repository.UpdateBook(book);
                }
                // book.Status = "Completed"
                // else book.Status = "Cancelled"
                // update book in db
                        
            }
        }
    }
}
