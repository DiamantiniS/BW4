using Microsoft.AspNetCore.Mvc;
using BW4_progetto.Models;
using BW4_progetto.Services;

namespace BW4_progetto.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            return PartialView("~/Views/Product/Details.cshtml", product);
        }
    }
}
