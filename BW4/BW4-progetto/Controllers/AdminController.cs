using Microsoft.AspNetCore.Mvc;
using BW4_progetto.Models;
using BW4_progetto.Services;

namespace BW4_progetto.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly DatabaseService _databaseService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ProductService productService, IWebHostEnvironment hostingEnvironment, DatabaseService databaseService, ILogger<AdminController> logger)
        {
            _productService = productService;
            _hostingEnvironment = hostingEnvironment;
            _databaseService = databaseService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid.");

                if (imageFile != null && imageFile.Length > 0)
                {
                    _logger.LogInformation("Image file provided.");
                    var uniqueFileName = await UploadImage(imageFile);
                    product.ImageUrl = "/uploads/" + uniqueFileName;
                }
                else
                {
                    _logger.LogInformation("No image file provided.");
                }

                _productService.AddProduct(product);
                _logger.LogInformation("Product added successfully.");
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Model state is not valid.");
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        private async Task<string> UploadImage(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return uniqueFileName;
        }
    }
}
