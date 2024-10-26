using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    public class BookController: ControllerBase <BookController>
    {
        private readonly IBookService _bookService;
        public BookController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            IBookService bookService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookService = bookService;     
        }
        public IActionResult Index()
        {
            return View();
        }
    }
    
}
