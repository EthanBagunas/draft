﻿using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public LoginResult AuthenticateUser(string userId, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().Where(x => x.UserId == userId &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            try { 
            
                    _mapper.Map(model, user);
                    user.Password = PasswordManager.EncryptPassword(model.Password);
                    user.CreatedTime = DateTime.Now;
                    user.UpdatedTime = DateTime.Now;
                    user.CreatedBy = System.Environment.UserName;
                    user.UpdatedBy = System.Environment.UserName;

                    _repository.AddUser(user);
                }

                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("An InvalidOperationException occurred: " + ex.Message);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected exception occurred: " + ex.Message);
                }

            /*if (!_repository.UserExists(model.UserId))
            { }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }*/
        }
        public void DeleteUser(UserViewModel model)
        {
    
            _repository.DeleteUser(model.UserId);
        }
        public void UpdateUser(EditUserViewModel model)
        {
            //var user = new User();
            var user = _repository.GetUsers().FirstOrDefault(x => x.UserId == model.UserId);

            // Check if the user was found
            if (user == null)
            {
                throw new ArgumentException($"User  with ID {model.UserId} not found.");
            }
            try
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.UpdatedTime = DateTime.Now;
                user.UpdatedBy = System.Environment.UserName;
                _repository.UpdateUser(user);
            }

            catch (Exception ex)
            {
                Console.WriteLine("An unexpected exception occurred: " + ex.Message);
            }
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _repository.GetAll().ToList();
        }
    }
}
