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
using System.Threading.Tasks;
using System.Linq;
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
            var rooms = _roomService.GetAllRooms();
            var bookings = _bookService.GetAllBooks();
            var statuses = _roomService.GetCurrentRoomStatuses();

            var viewModel = new HomepageViewModel
            {
                Rooms = rooms.ToList(),
                Bookings = bookings.ToList(),
                RoomStatuses = statuses
            };

            return View(viewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult NewRoom(RoomViewModel model)
        {
            try
            {
                _roomService.AddRoom(model);
                return Json(new { success = true, message = "Room added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding room: {ex.Message}");
                return Json(new { success = false, message = $"Failed to add room: {ex.Message}" });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult DeleteRoom(int roomId)
        {
            try
            {
                _roomService.DeleteRoom(roomId);
                return Json(new { success = true, message = "Room deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting room: {ex.Message}");
                return Json(new { success = false, message = $"Failed to delete room: {ex.Message}" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Room>> GetAllRooms()
        {
            var rooms = _roomService.GetAllRooms();
            return Ok(rooms); // Returns a 200 OK response with the list of rooms
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult NewBook(BookViewModel model)
        {
            try
            {
                _bookService.AddBook(model);
                return Json(new { success = true, message = "Booking added successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult UpdateBook([FromQuery] int bookid)
        {
            _bookService.UpdateBook(bookid);
            return Ok();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetBookingsbyRoomid([FromQuery] int roomid)

        {
            var books = _bookService.GetAllBooksbyId(roomid);

            
            return Json(books);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetRoom(int id)
        {
            try
            {
                var room = _roomService.GetRoomById(id);
                return Json(new { success = true, data = room });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting room: {ex.Message}");
                return Json(new { success = false, message = $"Failed to get room: {ex.Message}" });
            }
        }

    }
}