using BW4_progetto.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace BW4_progetto.Services
{
    public class ProductService
    {
        private readonly DatabaseService _databaseService;

        public ProductService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            using (var connection = _databaseService.GetConnection())
            {
                return connection.Query<Product>("SELECT * FROM Products").ToList();
            }
        }

        public Product GetProductById(int id)
        {
            using (var connection = _databaseService.GetConnection())
            {
                return connection.QuerySingleOrDefault<Product>("SELECT * FROM Products WHERE ProductId = @Id", new { Id = id });
            }
        }

        public void AddProduct(Product product)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var sql = "INSERT INTO Products (Name, Description, Price, ImageUrl) VALUES (@Name, @Description, @Price, @ImageUrl)";
                connection.Execute(sql, product);
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var sql = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, ImageUrl = @ImageUrl WHERE ProductId = @ProductId";
                connection.Execute(sql, product);
            }
        }

        public void DeleteProduct(int id)
        {
            using (var connection = _databaseService.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _logger.LogInformation("Deleting references in CartItems for Product ID {ProductId}", id);
                        connection.Execute("DELETE FROM CartItems WHERE ProductId = @Id", new { Id = id }, transaction);

                        _logger.LogInformation("Deleting product with ID {ProductId}", id);
                        connection.Execute("DELETE FROM Products WHERE ProductId = @Id", new { Id = id }, transaction);

                        transaction.Commit();
                        _logger.LogInformation("Successfully deleted product with ID {ProductId}", id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool HasReferences(int productId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                var count = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM CartItems WHERE ProductId = @ProductId", new { ProductId = productId });
                return count > 0;
            }
        }
    }
}
