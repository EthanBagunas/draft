using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookService
    {
        void AddBook(BookViewModel model);
        void UpdateBook(int  bookid);

        IEnumerable<Book> GetAllBooksbyId(int roomid);


       // IEnumerable<Book> GetRoomWithBookingsAsync(int roomid);

    }
}