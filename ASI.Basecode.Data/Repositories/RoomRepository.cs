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

        public void  AddRoom(RoomInformation room)
        {
            this.GetDbSet<RoomInformation>().Add(room);
            UnitOfWork.SaveChanges();
        }
        public void UpdateRoom(RoomInformation room)

        {
            this.GetDbSet<RoomInformation>().Update(room);
            UnitOfWork.SaveChanges();

        }
    }
}
