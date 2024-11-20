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
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AccountController : ControllerBase<AccountController>
    {
        private readonly SessionManager _sessionManager;
        private readonly SignInManager _signInManager;
        private readonly TokenValidationParametersFactory _tokenValidationParametersFactory;
        private readonly TokenProviderOptionsFactory _tokenProviderOptionsFactory;
        private readonly IConfiguration _appConfiguration;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IBookService _bookService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="tokenValidationParametersFactory">The token validation parameters factory.</param>
        /// <param name="tokenProviderOptionsFactory">The token provider options factory.</param>
        /// <param name="roomService">The room service.</param>
        /// <param name="bookService">The book service.</param>
        public AccountController(
                            SignInManager signInManager,
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper,
                            IUserService userService,
                            TokenValidationParametersFactory tokenValidationParametersFactory,
                            TokenProviderOptionsFactory tokenProviderOptionsFactory,
                            IRoomService roomService,
                            IBookService bookService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._sessionManager = new SessionManager(this._session);
            this._signInManager = signInManager;
            this._tokenProviderOptionsFactory = tokenProviderOptionsFactory;
            this._tokenValidationParametersFactory = tokenValidationParametersFactory;
            this._appConfiguration = configuration;
            this._userService = userService;
            this._roomService = roomService;
            this._bookService = bookService;


        }

        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns>Created response view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            TempData["returnUrl"] = System.Net.WebUtility.UrlDecode(HttpContext.Request.Query["ReturnUrl"]);
            this._sessionManager.Clear();
            this._session.SetString("SessionId", System.Guid.NewGuid().ToString());
            return this.View();
        }

        /// <summary>
        /// Authenticate user and signs the user in when successful.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns> Created response view </returns>
        /// 

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try 
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for user {UserId}", model.UserId);
                    return Json(new { success = false, message = "Invalid input" });
                }

                _logger.LogInformation("Attempting login for user {UserId}", model.UserId);
                
                User user = new User();
                var loginResult = _userService.AuthenticateUser(model.UserId, model.Password, ref user);
                
                if (loginResult == LoginResult.Success && user != null)
                {
                    this._session.SetString("SessionId", System.Guid.NewGuid().ToString());
                    this._session.SetString("HasSession", "Exist");
                    await this._signInManager.SignInAsync(user);
                    this._session.SetString("UserName", user.Name);

                    return Json(new { 
                        success = true, 
                        redirectUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Homepage", "Home") 
                    });
                }
                
                return Json(new { 
                    success = false, 
                    message = "Incorrect UserId or Password" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user {UserId}", model.UserId);
                return Json(new { 
                    success = false, 
                    message = "An error occurred during login" 
                });
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccountInfo()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user information");
                TempData["ErrorMessage"] = "Error retrieving user information";
                return View(Enumerable.Empty<User>());
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AdminRoom()
        {
            var rooms = _roomService.GetAllRooms();
            return View(rooms);
        }

        
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(UserViewModel model)
        {
            try
            {
                _userService.AddUser(model);
                return RedirectToAction("Login", "Account");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();
        }


        
        [HttpPost]
        [AllowAnonymous]
        public IActionResult UpdateUser(EditUserViewModel model)
        {

            try 
            {
                Console.WriteLine(model);
                _userService.UpdateUser(model);

                return RedirectToAction("Login", "Account");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult DeleteUser(UserViewModel user)
        {
            try
            {
                _userService.DeleteUser(user);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error {e.Message}");
            }
            return Ok();
        }

        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
       

        /// <summary>
        /// Sign Out current account and return login view.
        /// </summary>
        /// <returns>Created response view</returns>
        [AllowAnonymous]
        public async Task<IActionResult> SignOutUser()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Name = $"{model.Fname} {model.Lname}";
                    model.CreatedTime = DateTime.Now;
                    model.CreatedBy = User.Identity?.Name ?? "System";

                    _userService.AddUser(model);
                    return Json(new { success = true });
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetBookingsbyRoomid([FromQuery] int roomid)
        {
            try
            {
                var books = _bookService.GetAllBooksbyId(roomid)
                                       .Where(b => b.Status == "RESERVED" || b.Status == "VACANT")
                                       .OrderBy(b => b.BookingDate)
                                       .ThenBy(b => b.TimeIn);
                
                return Json(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching bookings: {ex.Message}");
                return Json(new { success = false, message = "Failed to fetch bookings" });
            }
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllBookings()
        {
            try
            {
                var books = _bookService.GetAllBooks()
                                       .OrderBy(b => b.RoomId)
                                       .ThenBy(b => b.BookingDate)
                                       .ThenBy(b => b.TimeIn)
                                       .ToList();
                
                return Json(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching all bookings: {ex.Message}");
                return Json(new { success = false, message = "Failed to fetch bookings" });
            }
        }
    }
}
