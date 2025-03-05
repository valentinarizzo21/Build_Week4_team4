using BeviSano.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BeviSano.Controllers
{
    public class CartController : Controller
    {
        private readonly string _connectionString;

        public CartController()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            ProductListCart productsList = new ProductListCart();
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Cart INNER JOIN Products ON Cart.Id_Product = Products.Id_Product WHERE Id_Cart = @id_account";
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

                string selectQuery = "SELECT Quantity_Product FROM Cart WHERE Id_Product = @id_product AND Id_Cart = @id_account";
                int currentQuantity = 0;
                await using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@id_product", id);
                    selectCommand.Parameters.AddWithValue("@id_account", HomeController.MainAccount.Id);
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
                    string updateQuery = "UPDATE Cart SET Quantity_Product = Quantity_Product - 1 WHERE Id_Product = @id_product AND Id_Cart = @id_account";
                    await using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@id_product", id);
                        updateCommand.Parameters.AddWithValue("@id_account", HomeController.MainAccount.Id);

                        await updateCommand.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    string deleteQuery = "DELETE FROM Cart WHERE Id_Product = @id_product AND Id_Cart = @id_account";

                    await using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@id_product", id);
                        deleteCommand.Parameters.AddWithValue("@id_account", HomeController.MainAccount.Id);

                        await deleteCommand.ExecuteNonQueryAsync();
                    }
                }
            }
            return RedirectToAction("Index");
        }

    }
}
