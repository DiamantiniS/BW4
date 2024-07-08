using Microsoft.AspNetCore.Mvc;

namespace BW4.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
