using Microsoft.AspNetCore.Mvc;
using BW4_progetto.Models;
using BW4_progetto.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BW4_progetto.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ProductService productService, ILogger<AdminController> logger)
        {
            _productService = productService;
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
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid.");
                _productService.AddProduct(product);
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
            _productService.DeleteProduct(id);
             return Json(new { success = true });
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                // Gestione dell'errore: nessun file selezionato
                return RedirectToAction("Error");
            }

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Verifica se la cartella uploads esiste, altrimenti creala
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Salva il file sul disco
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Salvataggio delle informazioni dell'immagine nel database
            // var connectionString = _configuration.GetConnectionString("DefaultConnection");


            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                var query = "INSERT INTO Images (FileName, FilePath) VALUES (@FileName, @FilePath)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FileName", imageFile.FileName);
                    command.Parameters.AddWithValue("@FilePath", filePath);
                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }*/
    }
}
