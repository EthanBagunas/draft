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
       public void DoWork()
    {
            Console.WriteLine("MyHostedService is working.");
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
        protected void Application_Start()
        {
            // Other startup code...
            _timer = new Timer(60000); // Set interval to 1 minute (60000 milliseconds)
            _timer.Elapsed += UpdateCompletedBooks;
            _timer.Start();
        }


        private void UpdateCompletedBooks(object sender, ElapsedEventArgs e)

        {
            var books = _repository.GetAllBooks();
            var currentTime = DateTime.Now;

            foreach (var book in books)

            {
                if (book.BookingDate == currentTime.Date)
                {

                    if (book.TimeOut > currentTime.TimeOfDay)

                    {

                        book.Status = "Completed";

                        _repository.UpdateBook(book);

                    }

                }

            }

        }


        protected void Application_End()

        {

            _timer?.Stop();

            _timer?.Dispose();

        }
    }
}
