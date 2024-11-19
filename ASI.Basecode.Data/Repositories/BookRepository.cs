using ASI.Basecode.Data.Interfaces;
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

        public IEnumerable<Book> GetAllBooks()
        {
            return this.GetDbSet<Book>()
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .ToList();
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
    }
}
