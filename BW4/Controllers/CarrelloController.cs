using Microsoft.AspNetCore.Mvc;

namespace BW4.Controllers
{
    public class CarrelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
