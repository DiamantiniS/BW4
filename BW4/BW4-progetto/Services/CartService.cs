using BW4_progetto.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace BW4_progetto.Services
{
    public class CartService
    {
        private readonly DatabaseService _databaseService;
        private readonly ProductService _productService;

        public CartService(DatabaseService databaseService, ProductService productService)
        {
            _databaseService = databaseService;
            _productService = productService;
        }

        public void AddToCart(int productId, int quantity)
        {
            using (var connection = _databaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Check if a cart exists, otherwise create a new cart
                        var cartId = connection.QuerySingleOrDefault<int>(
                            "SELECT TOP 1 CartId FROM Carts ORDER BY CreatedDate DESC",
                            transaction: transaction);

                        if (cartId == 0)
                        {
                            cartId = connection.QuerySingle<int>(
                                "INSERT INTO Carts (CreatedDate) OUTPUT INSERTED.CartId VALUES (GETDATE())",
                                transaction: transaction);
                        }

                        // Check if the item already exists in the cart
                        var existingItem = connection.QuerySingleOrDefault<CartItem>(
                            "SELECT * FROM CartItems WHERE CartId = @CartId AND ProductId = @ProductId",
                            new { CartId = cartId, ProductId = productId },
                            transaction: transaction);

                        if (existingItem != null)
                        {
                            // Update the quantity of the existing item
                            connection.Execute(
                                "UPDATE CartItems SET Quantity = Quantity + @Quantity WHERE CartItemId = @CartItemId",
                                new { Quantity = quantity, CartItemId = existingItem.CartItemId },
                                transaction: transaction);
                        }
                        else
                        {
                            // Add a new item to the cart
                            connection.Execute(
                                "INSERT INTO CartItems (CartId, ProductId, Quantity) VALUES (@CartId, @ProductId, @Quantity)",
                                new { CartId = cartId, ProductId = productId, Quantity = quantity },
                                transaction: transaction);
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateCartItem(int cartItemId, int quantity)
        {
            using (var connection = _databaseService.GetConnection())
            {
                connection.Execute(
                    "UPDATE CartItems SET Quantity = @Quantity WHERE CartItemId = @CartItemId",
                    new { Quantity = quantity, CartItemId = cartItemId });
            }
        }

        public void RemoveFromCart(int cartItemId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                connection.Execute("DELETE FROM CartItems WHERE CartItemId = @CartItemId", new { CartItemId = cartItemId });
            }
        }

        public void ClearCart(int cartId)
        {
            using (var connection = _databaseService.GetConnection())
            {
                connection.Execute("DELETE FROM CartItems WHERE CartId = @CartId", new { CartId = cartId });
            }
        }

        public Cart GetCart()
        {
            using (var connection = _databaseService.GetConnection())
            {
                var cart = connection.QuerySingleOrDefault<Cart>("SELECT TOP 1 * FROM Carts ORDER BY CreatedDate DESC");

                if (cart != null)
                {
                    cart.Items = connection.Query<CartItem, Product, CartItem>(
                        "SELECT ci.*, p.* FROM CartItems ci INNER JOIN Products p ON ci.ProductId = p.ProductId WHERE ci.CartId = @CartId",
                        (ci, p) => { ci.Product = p; return ci; },
                        new { CartId = cart.CartId },
                        splitOn: "ProductId").ToList();

                    if (cart.Items == null)
                    {
                        cart.Items = new List<CartItem>();
                    }
                }
                else
                {
                    cart = new Cart { Items = new List<CartItem>() };
                }

                return cart;
            }
        }
    }
}
