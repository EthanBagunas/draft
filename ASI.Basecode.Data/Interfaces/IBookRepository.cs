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
        void CreateBook(Reservation reservation);
        void UpdateBook(Reservation reservation);
    }
}
