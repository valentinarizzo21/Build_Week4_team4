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

        public async Task <IActionResult> Index()
        {
            var Products = new StoreList();
            await using (SqlConnection connection = new SqlConnection(_connectionString)) {
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
                            }
                        ;
                            Products.Products.Add(product);
                        }
                    }
                }
            }
                return View(Products);
        }
    }
}
