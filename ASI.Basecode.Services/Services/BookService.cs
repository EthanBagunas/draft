﻿using ASI.Basecode.Data.Interfaces;
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

namespace ASI.Basecode.Services.Services
{
    public  class BookService: IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IRoomService _roomService;
        private static Timer _timer;

        public BookService(IBookRepository repository, IRoomService roomService) 
        {
            _repository = repository;
            _roomService = roomService;
        }
        public void DoWork()
        {
            Console.WriteLine("MyHostedService is working.");
        }
        public void AddBook(BookViewModel model)
        {
            // TimeSpan validation
            if (model.TimeIn < TimeSpan.FromHours(8) || 
                model.TimeOut > TimeSpan.FromHours(21))
            {
                throw new ArgumentException("Booking times must be between 8:00 AM and 9:00 PM");
            }

            // Check for existing bookings
            var existingBooking = _repository.GetAllBooks()
                .Any(b => b.RoomId == model.RoomId && 
                          b.BookingDate.HasValue && 
                          b.BookingDate.Value.Date == model.BookingDate.Date && 
                          b.TimeIn == model.TimeIn);

            if (existingBooking)
            {
                throw new InvalidOperationException("This time slot is already booked");
            }

            var book = new Book
            {
                RoomId = model.RoomId,
                BookingDate = model.BookingDate,
                TimeIn = model.TimeIn,
                TimeOut = model.TimeOut,
                Status = "RESERVED",
                Duration = (int)(model.TimeOut - model.TimeIn).TotalHours,
                ReservationDate = DateTime.Now.Date
            };

            _repository.CreateBook(book);
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

        public Dictionary<int, string> GetCurrentRoomStatuses()
        {
            var currentTime = DateTime.Now;
            var statuses = new Dictionary<int, string>();
            var rooms = _roomService.GetAllRooms();

            foreach (var room in rooms)
            {
                var currentBooking = _repository.GetAllBooks()
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

                if (currentBooking.TimeIn.HasValue && currentTime.TimeOfDay < currentBooking.TimeIn.Value)
                    statuses[room.Id] = "Reserved";
                else if (currentBooking.TimeIn.HasValue && currentBooking.TimeOut.HasValue && 
                         currentTime.TimeOfDay >= currentBooking.TimeIn.Value && 
                         currentTime.TimeOfDay <= currentBooking.TimeOut.Value)
                    statuses[room.Id] = "Occupied";
                else
                    statuses[room.Id] = "Vacant";
            }

            return statuses;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _repository.GetAllBooks();
        }
    }
}
