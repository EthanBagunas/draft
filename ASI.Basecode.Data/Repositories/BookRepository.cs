﻿using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

       
        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void CreateBook(Book book)
        {
            this.GetDbSet<Book>().Add(book);
            UnitOfWork.SaveChanges();
        }
        public void UpdateBook(Book book)
        {
            this.GetDbSet<Book>().Update(book);
            UnitOfWork.SaveChanges();
        }
        public IEnumerable<Book> GetAllBooks()
        {
            return this.GetDbSet<Book>();
        }
        /*public async Task<Room> GetRoomWithBookingsAsync(int roomId)
        {
          
        }*/

    }
}
