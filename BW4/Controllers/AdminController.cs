using BW4.Interaface;
using BW4.Models;
using Microsoft.AspNetCore.Mvc;

namespace BW4.Controllers
{
    public class AdminController : Controller
    {
        private readonly iProdottoService _prodottoService;

        public AdminController(iProdottoService prodottoService)
        {
            _prodottoService = prodottoService;
        }

        public IActionResult Index()
        {
            var prodotti = _prodottoService.TuttiProdotti();
            return View(prodotti);
        }

        public IActionResult Crea()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crea(Prodotto prodotto)
        {
            if (ModelState.IsValid)
            {
                _prodottoService.CreaProdotto(prodotto);
                return RedirectToAction("Index");
            }
            return View(prodotto);
        }

        public IActionResult Modifica(int id)
        {
            var prodotto = _prodottoService.SingoloProdotto(id);
            if (prodotto == null)
            {
                return NotFound();
            }
            return View(prodotto);
        }

        [HttpPost]
        public IActionResult Modifica(int id, Prodotto prodotto)
        {
            if (ModelState.IsValid)
            {
                _prodottoService.ModificaProdotto(id, prodotto);
                return RedirectToAction("Index");
            }
            return View(prodotto);
        }

        [HttpPost]
        public IActionResult Elimina(int id)
        {
            _prodottoService.EliminaProdotto(id);
            return RedirectToAction("Index");
        }
    }
}
