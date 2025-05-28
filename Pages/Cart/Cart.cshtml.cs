using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Security.Claims;

namespace HardwareStore_Application.Pages
{

    public class CartModel : PageModel
    {
        private readonly IConfiguration _config;
        public string ConnectionString { get; private set; } = string.Empty;

        public CartModel(IConfiguration config)
        {
            _config = config;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }

        public async Task OnGetAsync()
        {
            ConnectionString = _config.GetConnectionString("DefaultConnection") ??
                            "server=localhost;user=root;password=;database=hardwarestore;";

            await LoadCartItems();
            CalculateTotals();
        }

        // POST handler for adding item to cart
        public async Task<IActionResult> OnPostAddToCartAsync([FromBody] AddToCartRequest request)
        {
            try
            {
                ConnectionString = _config.GetConnectionString("DefaultConnection") ??
                                "server=localhost;user=root;password=;database=hardwarestore;";

                // Get product details from database
                var product = await GetProductById(request.ProductId);
                if (product == null)
                {
                    return new JsonResult(new { success = false, message = "Product not found" });
                }

                await LoadCartItems();

                // Check if item already exists in cart
                var existingItem = CartItems.FirstOrDefault(i => i.ProductId == request.ProductId);
                if (existingItem != null)
                {
                    // Update quantity
                    existingItem.Quantity += request.Quantity;
                    existingItem.TotalPrice = existingItem.Price * existingItem.Quantity;
                }
                else
                {
                    // Add new item
                    CartItems.Add(new CartItem
                    {
                        ProductId = product.ProductID,
                        Name = product.ProductName,
                        Description = $"{product.CategoryName} - {product.BrandName}",
                        Price = product.Price,
                        Quantity = request.Quantity,
                        IconClass = GetIconForCategory(product.CategoryName),
                        TotalPrice = product.Price * request.Quantity
                    });
                }

                // Update session
                HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(CartItems));

                // Update database if user is authenticated
                if (User.Identity?.IsAuthenticated == true)
                {
                    await UpdateCartInDatabase();
                }

                return new JsonResult(new
                {
                    success = true,
                    cartCount = CartItems.Sum(i => i.Quantity),
                    message = "Item added to cart successfully!"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to cart: {ex.Message}");
                return new JsonResult(new { success = false, message = "Error adding item to cart" });
            }
        }

        // POST handler for updating quantity
        public async Task<IActionResult> OnPostUpdateQuantityAsync([FromBody] UpdateQuantityRequest request)
        {
            try
            {
                ConnectionString = _config.GetConnectionString("DefaultConnection") ??
                                "server=localhost;user=root;password=;database=hardwarestore;";
                await LoadCartItems();

                var item = CartItems.FirstOrDefault(i => i.Id == request.ItemId);
                if (item != null)
                {
                    item.Quantity = request.Quantity;
                    item.TotalPrice = item.Price * item.Quantity;

                    // Update session
                    HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(CartItems));

                    // Update database if user is authenticated
                    if (User.Identity?.IsAuthenticated == true)
                    {
                        await UpdateCartInDatabase();
                    }

                    return new JsonResult(new { success = true });
                }

                return new JsonResult(new { success = false, message = "Item not found" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating quantity: {ex.Message}");
                return new JsonResult(new { success = false, message = "Error updating quantity" });
            }
        }

        // POST handler for removing item
        public async Task<IActionResult> OnPostRemoveItemAsync([FromBody] RemoveItemRequest request)
        {
            try
            {
                ConnectionString = _config.GetConnectionString("DefaultConnection") ??
                                "server=localhost;user=root;password=;database=hardwarestore;";
                await LoadCartItems();

                var item = CartItems.FirstOrDefault(i => i.Id == request.ItemId);
                if (item != null)
                {
                    CartItems.Remove(item);

                    // Update session
                    HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(CartItems));

                    // Update database if user is authenticated
                    if (User.Identity?.IsAuthenticated == true)
                    {
                        await UpdateCartInDatabase();
                    }

                    return new JsonResult(new { success = true });
                }

                return new JsonResult(new { success = false, message = "Item not found" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item: {ex.Message}");
                return new JsonResult(new { success = false, message = "Error removing item" });
            }
        }

        // POST handler for clearing cart
        public async Task<IActionResult> OnPostClearCartAsync()
        {
            try
            {
                ConnectionString = _config.GetConnectionString("DefaultConnection") ??
                                "server=localhost;user=root;password=;database=hardwarestore;";

                CartItems.Clear();
                HttpContext.Session.Remove("CartItems");

                if (User.Identity?.IsAuthenticated == true)
                {
                    await ClearCartInDatabase();
                }

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart: {ex.Message}");
                return new JsonResult(new { success = false, message = "Error clearing cart" });
            }
        }

        // POST handler for checkout
        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            try
            {
                if (!User.Identity?.IsAuthenticated == true)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        requiresLogin = true,
                        message = "Please log in to complete your order"
                    });
                }

                ConnectionString = _config.GetConnectionString("DefaultConnection") ??
                                "server=localhost;user=root;password=;database=hardwarestore;";

                await LoadCartItems();

                if (!CartItems.Any())
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Your cart is empty"
                    });
                }

                // Process the order (save to orders table)
                var orderId = await ProcessOrder();

                if (orderId > 0)
                {
                    // Clear cart after successful order
                    CartItems.Clear();
                    HttpContext.Session.Remove("CartItems");
                    await ClearCartInDatabase();

                    return new JsonResult(new
                    {
                        success = true,
                        orderId = orderId,
                        message = "Thank you for your order! Order confirmation has been sent."
                    });
                }

                return new JsonResult(new
                {
                    success = false,
                    message = "Error processing your order. Please try again."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during checkout: {ex.Message}");
                return new JsonResult(new
                {
                    success = false,
                    message = "Error processing your order. Please try again."
                });
            }
        }

        public async Task<ProductDetails> GetProductById(int productId)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                await connection.OpenAsync();

                var query = @"
                    SELECT p.ProductID, p.ProductName, p.Price, c.CategoryName, b.BrandName
                    FROM product p
                    INNER JOIN category c ON p.CategoryID = c.CategoryID
                    INNER JOIN brand b ON p.BrandID = b.BrandID
                    WHERE p.ProductID = @ProductId";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new ProductDetails
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        ProductName = reader.GetString("ProductName"),
                        Price = reader.GetDecimal("Price"),
                        CategoryName = reader.GetString("CategoryName"),
                        BrandName = reader.GetString("BrandName")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product: {ex.Message}");
            }

            return null;
        }

        public async Task LoadCartItems()
        {
            try
            {
                // Initialize CartItems if null
                CartItems ??= new List<CartItem>();

                // Try to get cart items from session
                var sessionCart = HttpContext?.Session?.GetString("CartItems");
                if (!string.IsNullOrEmpty(sessionCart))
                {
                    var deserializedItems = JsonSerializer.Deserialize<List<CartItem>>(sessionCart);
                    if (deserializedItems != null)
                    {
                        CartItems = deserializedItems;
                    }
                }
                else if (User.Identity?.IsAuthenticated == true)
                {
                    // If no session cart and user is authenticated, load from database
                    await LoadCartFromDatabase();
                    if (CartItems.Any())
                    {
                        HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(CartItems));
                    }
                }

                // Ensure all items have unique IDs for session-based carts
                for (int i = 0; i < CartItems.Count; i++)
                {
                    if (CartItems[i].Id == 0)
                    {
                        CartItems[i].Id = i + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cart items: {ex.Message}");
                CartItems = new List<CartItem>();
            }
        }

        public async Task LoadCartFromDatabase()
        {
            if (string.IsNullOrEmpty(ConnectionString)) return;

            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                await connection.OpenAsync();

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return;

                var query = @"
                    SELECT ci.CartItemID, ci.ProductID, ci.Quantity, p.ProductName, p.Price, 
                           c.CategoryName, b.BrandName
                    FROM cartitems ci
                    INNER JOIN product p ON ci.ProductID = p.ProductID
                    INNER JOIN category c ON p.CategoryID = c.CategoryID
                    INNER JOIN brand b ON p.BrandID = b.BrandID
                    WHERE ci.UserID = @UserId";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var price = reader.GetDecimal("Price");
                    var quantity = reader.GetInt32("Quantity");

                    CartItems.Add(new CartItem
                    {
                        Id = reader.GetInt32("CartItemID"),
                        ProductId = reader.GetInt32("ProductID"),
                        Name = reader.GetString("ProductName"),
                        Description = $"{reader.GetString("CategoryName")} - {reader.GetString("BrandName")}",
                        Price = price,
                        Quantity = quantity,
                        IconClass = GetIconForCategory(reader.GetString("CategoryName")),
                        TotalPrice = price * quantity
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cart from database: {ex.Message}");
            }
        }

        public async Task UpdateCartInDatabase()
        {
            if (string.IsNullOrEmpty(ConnectionString)) return;

            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                await connection.OpenAsync();

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return;

                // Clear existing cart items for this user
                var deleteQuery = "DELETE FROM cartitems WHERE UserID = @UserId";
                using var deleteCommand = new MySqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@UserId", userId);
                await deleteCommand.ExecuteNonQueryAsync();

                // Insert updated cart items
                foreach (var item in CartItems)
                {
                    var insertQuery = @"
                        INSERT INTO cartitems (UserID, ProductID, Quantity)
                        VALUES (@UserId, @ProductId, @Quantity)";

                    using var insertCommand = new MySqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@UserId", userId);
                    insertCommand.Parameters.AddWithValue("@ProductId", item.ProductId);
                    insertCommand.Parameters.AddWithValue("@Quantity", item.Quantity);

                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating cart in database: {ex.Message}");
            }
        }

        public async Task ClearCartInDatabase()
        {
            if (string.IsNullOrEmpty(ConnectionString)) return;

            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                await connection.OpenAsync();

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return;

                var deleteQuery = "DELETE FROM cartitems WHERE UserID = @UserId";
                using var deleteCommand = new MySqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@UserId", userId);
                await deleteCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart in database: {ex.Message}");
            }
        }

        public async Task<int> ProcessOrder()
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                await connection.OpenAsync();

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return 0;

                CalculateTotals();

                // Create order
                var orderQuery = @"
                    INSERT INTO orders (UserID, OrderDate, Subtotal, Tax, ShippingCost, Total, Status)
                    VALUES (@UserId, @OrderDate, @Subtotal, @Tax, @ShippingCost, @Total, 'Pending');
                    SELECT LAST_INSERT_ID();";

                using var orderCommand = new MySqlCommand(orderQuery, connection);
                orderCommand.Parameters.AddWithValue("@UserId", userId);
                orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                orderCommand.Parameters.AddWithValue("@Subtotal", Subtotal);
                orderCommand.Parameters.AddWithValue("@Tax", Tax);
                orderCommand.Parameters.AddWithValue("@ShippingCost", ShippingCost);
                orderCommand.Parameters.AddWithValue("@Total", Total);

                var orderId = Convert.ToInt32(await orderCommand.ExecuteScalarAsync());

                // Create order items
                foreach (var item in CartItems)
                {
                    var itemQuery = @"
                        INSERT INTO orderitems (OrderID, ProductID, Quantity, Price, TotalPrice)
                        VALUES (@OrderId, @ProductId, @Quantity, @Price, @TotalPrice)";

                    using var itemCommand = new MySqlCommand(itemQuery, connection);
                    itemCommand.Parameters.AddWithValue("@OrderId", orderId);
                    itemCommand.Parameters.AddWithValue("@ProductId", item.ProductId);
                    itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                    itemCommand.Parameters.AddWithValue("@Price", item.Price);
                    itemCommand.Parameters.AddWithValue("@TotalPrice", item.TotalPrice);

                    await itemCommand.ExecuteNonQueryAsync();
                }

                return orderId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing order: {ex.Message}");
                return 0;
            }
        }

        public void CalculateTotals()
        {
            Subtotal = CartItems.Sum(item => item.TotalPrice);
            ShippingCost = Subtotal >= 100 ? 0 : 9.99m;
            Tax = Subtotal * 0.08m; // 8% tax rate
            Total = Subtotal + ShippingCost + Tax;
        }

        public string GetIconForCategory(string categoryName)
        {
            return categoryName?.ToLower() switch
            {
                "processors" => "fas fa-microchip",
                "graphics cards" => "fas fa-tv",
                "memory" => "fas fa-memory",
                "motherboards" => "fas fa-hdd",
                "storage" => "fas fa-hard-drive",
                "power supplies" => "fas fa-plug",
                "cases" => "fas fa-desktop",
                "cooling" => "fas fa-fan",
                _ => "fas fa-microchip"
            };
        }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string IconClass { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
    }

    public class ProductDetails
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
    }

    // Request models for the API calls
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateQuantityRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class RemoveItemRequest
    {
        public int ItemId { get; set; }
    }
}
