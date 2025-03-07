using BeviSano.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace BeviSano.Controllers
{
    public class CartController : Controller
    {
        private readonly string _connectionString;
        public List<CartProduct> CurrentCart = [];
        public static int Total = 0;
        public static decimal TotalPrice = 0;

        public CartController()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async void GetCurrentCart()
        {
            CurrentCart = [];

            if (HomeController.MainAccount != null)
            {
                await using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query =
                        "SELECT P.Id_Product, P.Name_Product, P.Price_Product, P.Sale_Product, C.Quantity_Product\r\nFROM Products as P\r\nINNER JOIN\r\nCart as C ON\r\nP.Id_Product = C.Id_Product\r\nWHERE C.Id_Cart = @id_account;";
                    await using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@id_account",
                            HomeController.MainAccount.Id.ToString()
                        );

                        await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var product = new CartProduct()
                                {
                                    Id_Product = reader.GetGuid(0),
                                    Name_Product = reader.GetString(1),
                                    Price_Product = reader.GetDecimal(2),
                                    Sale_Product = reader.GetDecimal(3),
                                    Quantity_Product = reader.GetInt32(4),
                                };
                                CurrentCart.Add(product);
                            }

                            GetCartQuantity();
                        }
                    }
                }
            }
        }

        public void GetCartQuantity()
        {
            int quantity = 0;
            decimal price = 0;

            foreach (var item in CurrentCart)
            {
                quantity += 1 * item.Quantity_Product;
                price +=
                    item.Price_Product * item.Quantity_Product * ((100 - item.Sale_Product) / 100);
            }

            Total = quantity;
            TotalPrice = price;
        }

        public async Task<IActionResult> Index()
        {
            ProductListCart productsList = new ProductListCart();
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query =
                    "SELECT * FROM Cart INNER JOIN Products ON Cart.Id_Product = Products.Id_Product WHERE Id_Cart = @id_account";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_account", HomeController.MainAccount.Id);
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CartProduct product1 = new CartProduct()
                            {
                                Id_Product = reader.GetGuid(3),
                                Name_Product = reader.GetString(5),
                                Price_Product = reader.GetDecimal(6),
                                Quantity_Product = reader.GetInt32(2),
                                Sale_Product = reader.GetDecimal(10),
                                Arrival_Date_Product = reader.GetInt32(11),
                                Cover_Product = reader.GetString(12),
                            };
                            productsList.Products.Add(product1);
                        }
                    }
                }
            }
            return View(productsList);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string selectQuery =
                    "SELECT Quantity_Product FROM Cart WHERE Id_Product = @id_product AND Id_Cart = @id_account";
                int currentQuantity = 0;
                await using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@id_product", id);
                    selectCommand.Parameters.AddWithValue(
                        "@id_account",
                        HomeController.MainAccount.Id
                    );
                    await using (SqlDataReader reader = await selectCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            currentQuantity = reader.GetInt32(0);
                        }
                    }
                }

                if (currentQuantity > 1)
                {
                    string updateQuery =
                        "UPDATE Cart SET Quantity_Product = Quantity_Product - 1 WHERE Id_Product = @id_product AND Id_Cart = @id_account";
                    await using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@id_product", id);
                        updateCommand.Parameters.AddWithValue(
                            "@id_account",
                            HomeController.MainAccount.Id
                        );

                        await updateCommand.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    string deleteQuery =
                        "DELETE FROM Cart WHERE Id_Product = @id_product AND Id_Cart = @id_account";

                    await using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@id_product", id);
                        deleteCommand.Parameters.AddWithValue(
                            "@id_account",
                            HomeController.MainAccount.Id
                        );

                        await deleteCommand.ExecuteNonQueryAsync();
                    }
                }
            }

            GetCurrentCart();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Order()
        {
            ProductListCart productsList = new ProductListCart();
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query =
                    "SELECT * FROM Cart INNER JOIN Products ON Cart.Id_Product = Products.Id_Product WHERE Id_Cart = @id_account";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_account", HomeController.MainAccount.Id);
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CartProduct product = new CartProduct()
                            {
                                Id_Product = reader.GetGuid(3),
                                Name_Product = reader.GetString(5),
                                Price_Product = reader.GetDecimal(6),
                                Quantity_Product = reader.GetInt32(2),
                                Sale_Product = reader.GetDecimal(10),
                                Arrival_Date_Product = reader.GetInt32(11),
                                Cover_Product = reader.GetString(12),
                            };
                            productsList.Products.Add(product);
                        }
                    }
                }

                foreach (var product in productsList.Products)
                {
                    int stock = 0;
                    string queryProduct =
                        "SELECT Stock_Product FROM Products WHERE Id_Product = @id";
                    await using (
                        SqlCommand commandProduct = new SqlCommand(queryProduct, connection)
                    )
                    {
                        commandProduct.Parameters.AddWithValue("@id", product.Id_Product);
                        await using (
                            SqlDataReader reader = await commandProduct.ExecuteReaderAsync()
                        )
                        {
                            if (await reader.ReadAsync())
                            {
                                stock = reader.GetInt32(0);
                            }
                        }
                    }

                    if (stock < product.Quantity_Product)
                    {
                        return RedirectToAction("Index");
                    }
                }

                foreach (var product in productsList.Products)
                {
                    string updateStock =
                        "UPDATE Products SET Stock_Product = Stock_Product - @quantity WHERE Id_Product = @id";
                    await using (SqlCommand commandStock = new SqlCommand(updateStock, connection))
                    {
                        commandStock.Parameters.AddWithValue("@quantity", product.Quantity_Product);
                        commandStock.Parameters.AddWithValue("@id", product.Id_Product);

                        await commandStock.ExecuteNonQueryAsync();
                    }
                }

                string deleteCart = "DELETE FROM Cart WHERE Id_Cart = @id";
                await using (SqlCommand commandDelete = new SqlCommand(deleteCart, connection))
                {
                    commandDelete.Parameters.AddWithValue("@id", HomeController.MainAccount.Id);

                    await commandDelete.ExecuteNonQueryAsync();
                }
            }
            GetCurrentCart();
            return View();
        }
    }
}
