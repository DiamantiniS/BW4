using BW4.Interafaces;
using Microsoft.AspNetCore.Mvc;

namespace BW4.Controllers
{
    public class CarrelloController : Controller
    {
        private readonly iCarrelloService _carrelloService;

        public CarrelloController(iCarrelloService carrelloService)
        {
            _carrelloService = carrelloService;
        }

        public IActionResult Index()
        {
            var carrello = _carrelloService.getCarrello();
            return View(carrello);
        }

        [HttpPost]
        public IActionResult Aggiungi(int prodottoId, int quantita)
        {
            _carrelloService.addCarrello(prodottoId, quantita);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Rimuovi(int id)
        {
            _carrelloService.deleteCarrello(id);
            return RedirectToAction("Index");
        }
    }
}
