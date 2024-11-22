using ASI.Basecode.Data.Interfaces;
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
            try
            {
                user = _repository.GetUsers()
                    .Where(x => x.UserId == userId)
                    .FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine("User not found");
                    return LoginResult.Failed;
                }

                // Check if password is stored in plain text (legacy)
                if (user.Password == password)
                {
                    // Update to encrypted password
                    user.Password = PasswordManager.EncryptPassword(password);
                    user.UpdatedTime = DateTime.Now;
                    _repository.UpdateUser(user);
                    return LoginResult.Success;
                }

                // Check encrypted password
                var passwordKey = PasswordManager.EncryptPassword(password);
                if (user.Password == passwordKey)
                {
                    return LoginResult.Success;
                }

                Console.WriteLine("Password mismatch");
                return LoginResult.Failed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                return LoginResult.Failed;
            }
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            try { 
            
                    _mapper.Map(model, user);
                    user.Password = PasswordManager.EncryptPassword(model.Password);
                    user.CreatedTime = DateTime.Now;
                    user.UpdatedTime = DateTime.Now;
                    user.Name = model.Fname + " " + model.Lname;
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

            var user = _repository.GetUsers().FirstOrDefault(x => x.Id == model.Id);
            if (user == null)
            {
                throw new ArgumentException($"User  with ID {model.Id} not found.");
            }
            user.Status = "INACTIVE";
            _repository.UpdateUser(user);
        }
        public void UpdateUser(EditUserViewModel model)
        {
            //var user = new User();
            var user = _repository.GetUsers().FirstOrDefault(x => x.Id == model.Id);

            // Check if the user was found
            if (user == null)
            {
                throw new ArgumentException($"User  with ID {model.Id} not found.");
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
            return _repository.GetAll().Where(u => u.Status != "INACTIVE").ToList();
        }
    }
}
