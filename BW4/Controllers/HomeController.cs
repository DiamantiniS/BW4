using BW4.Interaface;
using BW4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BW4.Controllers
{
    public class HomeController : Controller
    {
        private readonly iProdottoService _prodottoService;

        public HomeController(iProdottoService prodottoService)
        {
            _prodottoService = prodottoService;
        }

        public IActionResult Index()
        {
            var prodotti = _prodottoService.TuttiProdotti();
            return View(prodotti);
        }

        public IActionResult Details(int id)
        {
            var prodotto = _prodottoService.SingoloProdotto(id);
            if (prodotto == null)
            {
                return NotFound();
            }
            return View(prodotto);
        }
    }
}
