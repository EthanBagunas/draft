using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace ASI.Basecode.WebApp.Controllers
{
    
    public class DashboardController : Controller
    {
        

        [AllowAnonymous] //allows access to the dashboard publicly
        //next time, implement with the logic for login to redirect to this page
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}