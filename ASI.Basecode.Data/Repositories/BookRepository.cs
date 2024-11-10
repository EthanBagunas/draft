using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
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

        public IQueryable<Reservation> GetBookings()
        {
            return this.GetDbSet<Reservation>();
        }

        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void CreateBook(Reservation reservation)
        {
            this.GetDbSet<Reservation>().Add(reservation);
            UnitOfWork.SaveChanges();
        }
        public void UpdateBook(Reservation reservation)
        {
            this.GetDbSet<Reservation>().Update(reservation);
            UnitOfWork.SaveChanges();
        }

    }
}
