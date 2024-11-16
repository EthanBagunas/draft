using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : ControllerBase<HomeController>
    {
        private readonly IRoomService _roomService;
        private readonly IBookService _bookService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public HomeController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                               IMapper mapper,
                                IRoomService roomService,
                                IBookService bookService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._roomService = roomService; 
            this._bookService = bookService;
        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>
        /// 


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Homepage()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult NewRoom(RoomViewModel model)
        {
            var room = new Room();
            try
            {
                _roomService.AddRoom(model);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured:" + ex);
            }
            return Ok(room);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult NewBook(BookViewModel model) {
            var book = new Book();
            try
            {
                _bookService.AddBook(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured:" + ex);
            }
            return Ok(book);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Room>> GetAllRooms()
        {
            var rooms = _roomService.GetAllRooms();
            return Ok(rooms); // Returns a 200 OK response with the list of rooms
        }


    }
}
