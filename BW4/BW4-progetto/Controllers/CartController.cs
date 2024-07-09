using Microsoft.AspNetCore.Mvc;
using BW4_progetto.Models;
using BW4_progetto.Services;

namespace BW4_progetto.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }
    }
}
