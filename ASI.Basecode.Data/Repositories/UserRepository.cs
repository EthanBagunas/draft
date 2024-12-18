﻿using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {

        }

        public IQueryable<User> GetUsers()
        {
            return this.GetDbSet<User>();
        }

        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();

        }
        public void DeleteUser(string userid)
        {
            var entry = this.GetDbSet<User>().FirstOrDefault(e => e.UserId == userid);

            // Check if the user exists
            if (entry != null)
            {
                // Remove the user entity
                this.GetDbSet<User>().Remove(entry);
                UnitOfWork.SaveChanges();
            }
        }
        public IEnumerable<User> GetAll()
        {
            return this.GetDbSet<User>();
        }

    }
}
