using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 

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