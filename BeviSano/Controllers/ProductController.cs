using BeviSano.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BeviSano.Controllers
{
    public class ProductController : Controller
    {
        private readonly string _connectionString;

        public ProductController()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            var Products = new StoreList();
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Products";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = new Product
                            {
                                Id_Product = reader.GetGuid(0),
                                Name_Product = reader.GetString(1),
                                Price_Product = reader.GetDecimal(2),
                                Description_Product = reader.GetString(3),
                                Stock_Product = reader.GetInt32(4),
                                Seller_Product = reader.GetString(5),
                                Sale_Product = reader.GetDecimal(6),
                                Arrival_Date_Product = reader.GetInt32(7),
                                Cover_Product = reader.GetString(8),
                                Id_Category = reader.GetInt32(9),
                            };
                            Products.Products.Add(product);
                        }
                    }
                }
            }

            return View(Products);
        }

        [HttpGet("/Detail/{id:guid}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            SingleProduct product = new SingleProduct();
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Products WHERE Id_Product = @id";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            product.Id_Product = id;
                            product.Name_Product = reader.GetString(1);
                            product.Price_Product = reader.GetDecimal(2);
                            product.Description_Product = reader.GetString(3);
                            product.Stock_Product = reader.GetInt32(4);
                            product.Seller_Product = reader.GetString(5);
                            product.Sale_Product = reader.GetDecimal(6);
                            product.Arrival_Date_Product = reader.GetInt32(7);
                            product.Cover_Product = reader.GetString(8);
                            product.Id_Category = reader.GetInt32(9);
                        }
                    }
                }
            }
            return View(product);
        }

        public async Task<IActionResult> AddCart(Guid id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string checkQuery = "SELECT * FROM Cart WHERE Cart.Id_Product = @id AND Id_Cart = @account_id";
                await using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@id", id);
                    checkCommand.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                    await using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reader.Close();
                            string updateQuery = "UPDATE Cart SET Quantity_Product = Quantity_Product + 1 WHERE Id_Product = @id AND Id_Cart = @account_id";
                            await using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@id", id);
                                updateCommand.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                                await updateCommand.ExecuteNonQueryAsync();
                            }
                            return RedirectToAction("Index");
                        }
                        reader.Close();
                    }
                }

                string query = "INSERT INTO Cart (Id_Cart, Date_Add, Quantity_Product, Id_Product) VALUES (@account_id, @date_now, @quantity, @id_product)";
                await using (SqlCommand command = new SqlCommand(query, connection))
                { 
                    command.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                    command.Parameters.AddWithValue("@date_now", DateTime.Now);
                    command.Parameters.AddWithValue("@quantity", 1);
                    command.Parameters.AddWithValue("@id_product", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost("/Detail/Add/{id:guid}")]
        public async Task<IActionResult> AddCartDetail(Guid id, int quantity)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string checkQuery = "SELECT * FROM Cart WHERE Cart.Id_Product = @id AND Id_Cart = @account_id";
                await using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@id", id);
                    checkCommand.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                    await using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reader.Close();
                            string updateQuery = "UPDATE Cart SET Quantity_Product = Quantity_Product + @quantity WHERE Id_Product = @id AND Id_Cart = @account_id";
                            await using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@quantity", quantity);
                                updateCommand.Parameters.AddWithValue("@id", id);
                                updateCommand.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                                await updateCommand.ExecuteNonQueryAsync();
                            }
                            return RedirectToAction("Index");
                        }
                        reader.Close();
                    }
                }

                string query = "INSERT INTO Cart (Id_Cart, Date_Add, Quantity_Product, Id_Product) VALUES (@account_id, @date_now, @quantity, @id_product)";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                    command.Parameters.AddWithValue("@date_now", DateTime.Now);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@id_product", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
