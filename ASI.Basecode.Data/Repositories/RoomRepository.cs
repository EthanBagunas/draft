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
    public class RoomRepository: BaseRepository, IRoomRepository
    {
        public RoomRepository(IUnitOfWork unitOfWork): base(unitOfWork) { }

        public void  AddRoom(Room room)
        {
            this.GetDbSet<Room>().Add(room);
            UnitOfWork.SaveChanges();
        }
        public void UpdateRoom(Room room)

        {
            this.GetDbSet<Room>().Update(room);
            UnitOfWork.SaveChanges();

        }
        public IEnumerable<Room> GetAll()
        {
            return this.GetDbSet<Room>();
        }
    }
}
