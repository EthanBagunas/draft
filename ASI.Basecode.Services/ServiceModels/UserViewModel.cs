using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    // for register only
    public class UserViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string Fname { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        
        public string Lname { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public int Id { get; set; }
        public string Contact { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public string Name { get; set; }
    }
    public class EditUserViewModel 
    
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string Lname {  get; set; }
        public string Contact {  get; set; }   
        public string Password { get; set; }        
        public string ConfirmPassword { get; set; }
    }
}
