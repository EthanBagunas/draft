using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;


namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookRepository
    {
        //IQueryable<User> GetBookings();

        //bool UserExists(string userId);
        void CreateBook(Book book);
        void UpdateBook(Book book);
        IEnumerable<Book> GetAllBooks();
       // Task<Room> GetRoomWithBookingsAsync(int roomId);
    }
}
