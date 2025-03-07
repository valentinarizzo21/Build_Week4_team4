using BeviSano.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BeviSano.Controllers
{
    public class ProductController : Controller
    {
        private readonly string _connectionString;
        public static string searchQuery = "";
        public static int category = 0;

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

                if (category != 0)
                {
                    string query = "SELECT * FROM Products WHERE Id_Category = @id";
                    await using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", category);
                        await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var product = new Product()
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
                else
                {
                    string query = "SELECT * FROM Products";
                    await using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var product = new Product()
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

                string queryCategory = "SELECT * FROM Categories";
                await using (SqlCommand commandCategory = new SqlCommand(queryCategory, connection))
                {
                    await using (SqlDataReader reader = await commandCategory.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var category = new Category()
                            {
                                Id_Category = reader.GetInt32(0),
                                Title = reader.GetString(1),
                            };
                            Products.Categories.Add(category);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                Products.Products = Products
                    .Products.Where(p => p.Name_Product.ToLower().Contains(searchQuery.ToLower()))
                    .ToList();
            }

            return View(Products);
        }

        [HttpPost]
        public IActionResult UpdateVariable(string newValue)
        {
            searchQuery = newValue;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SelectCategory(int newCategory)
        {
            category = newCategory;
            searchQuery = "";
            return RedirectToAction("Index");
        }

        [HttpGet("/Detail/{id:guid}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            SingleProduct product = new SingleProduct();
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query =
                    "SELECT P.Id_Product, P.Name_Product, P.Price_Product, P.Description_Product, P.Stock_Product, P.Seller_Product, P.Sale_Product, P.Arrival_Date_Product, P.Id_Category, P.Cover_Product, C.Title FROM Products as P  INNER JOIN Categories as C ON P.Id_Category = C.Id_Category  WHERE P.Id_Product = @id;";
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
                            product.Id_Category = reader.GetInt32(8);
                            product.Cover_Product = reader.GetString(9);
                            product.Category_Name = reader.GetString(10);
                        }
                    }
                }

                string queryImgs = "SELECT Url_Image FROM Images WHERE Id_Product = @IdProduct;";
                await using (SqlCommand commandImgs = new SqlCommand(queryImgs, connection))
                {
                    commandImgs.Parameters.AddWithValue("@IdProduct", id);
                    await using (SqlDataReader reader = await commandImgs.ExecuteReaderAsync())
                    {
                        List<string> ImgsUrls = new List<string>();

                        while (await reader.ReadAsync())
                        {
                            ImgsUrls.Add(reader.GetString(0));
                        }

                        switch (ImgsUrls.Count)
                        {
                            case 1:
                                product.Url_Img_One = ImgsUrls[0];
                                break;

                            case 2:
                                product.Url_Img_One = ImgsUrls[0];
                                product.Url_Img_Two = ImgsUrls[1];
                                break;

                            case 3:
                                product.Url_Img_One = ImgsUrls[0];
                                product.Url_Img_Two = ImgsUrls[1];
                                product.Url_Img_Three = ImgsUrls[2];
                                break;

                            default:
                                break;
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

                string checkStockQuery =
                    "SELECT Stock_Product FROM Products WHERE Id_Product = @id";
                int stock = 0;
                await using (
                    SqlCommand checkStockCommand = new SqlCommand(checkStockQuery, connection)
                )
                {
                    checkStockCommand.Parameters.AddWithValue("@id", id);
                    await using (
                        SqlDataReader reader = await checkStockCommand.ExecuteReaderAsync()
                    )
                    {
                        if (await reader.ReadAsync())
                        {
                            stock = reader.GetInt32(0);
                        }
                    }
                }

                string checkQuantityQuery =
                    "SELECT Quantity_Product FROM Cart WHERE Id_Cart = @id_Account AND Id_Product = @id";
                int quantity = 0;
                await using (
                    SqlCommand checkQuantityCommand = new SqlCommand(checkQuantityQuery, connection)
                )
                {
                    checkQuantityCommand.Parameters.AddWithValue(
                        "@id_Account",
                        HomeController.MainAccount.Id
                    );
                    checkQuantityCommand.Parameters.AddWithValue("@id", id);
                    await using (
                        SqlDataReader reader = await checkQuantityCommand.ExecuteReaderAsync()
                    )
                    {
                        if (await reader.ReadAsync())
                        {
                            quantity = reader.GetInt32(0);
                        }
                    }
                }

                if (quantity >= stock)
                {
                    TempData["ErrorMessage"] = "Quantità non disponibile al momento";
                    TempData["idProduct"] = id;
                    return RedirectToAction("Index");
                }

                string checkQuery =
                    "SELECT * FROM Cart WHERE Cart.Id_Product = @id AND Id_Cart = @account_id";

                await using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@id", id);
                    checkCommand.Parameters.AddWithValue(
                        "@account_id",
                        HomeController.MainAccount.Id
                    );
                    await using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reader.Close();
                            string updateQuery =
                                "UPDATE Cart SET Quantity_Product = Quantity_Product + 1 WHERE Id_Product = @id AND Id_Cart = @account_id";
                            await using (
                                SqlCommand updateCommand = new SqlCommand(updateQuery, connection)
                            )
                            {
                                updateCommand.Parameters.AddWithValue("@id", id);
                                updateCommand.Parameters.AddWithValue(
                                    "@account_id",
                                    HomeController.MainAccount.Id
                                );
                                await updateCommand.ExecuteNonQueryAsync();
                            }

                            var newCartTwo = new CartController();
                            newCartTwo.GetCurrentCart();

                            return RedirectToAction("Index");
                        }
                        reader.Close();
                    }
                }

                string query =
                    "INSERT INTO Cart (Id_Cart, Date_Add, Quantity_Product, Id_Product) VALUES (@account_id, @date_now, @quantity, @id_product)";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                    command.Parameters.AddWithValue("@date_now", DateTime.Now);
                    command.Parameters.AddWithValue("@quantity", 1);
                    command.Parameters.AddWithValue("@id_product", id);
                    await command.ExecuteNonQueryAsync();
                }
            }

            var newCart = new CartController();
            newCart.GetCurrentCart();
            return RedirectToAction("Index");
        }

        [HttpPost("/Detail/Add/{id:guid}")]
        public async Task<IActionResult> AddCartDetail(Guid id, int quantity)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string checkStockQuery =
                    "SELECT Stock_Product FROM Products WHERE Id_Product = @id";
                int stock = 0;
                await using (
                    SqlCommand checkStockCommand = new SqlCommand(checkStockQuery, connection)
                )
                {
                    checkStockCommand.Parameters.AddWithValue("@id", id);
                    await using (
                        SqlDataReader reader = await checkStockCommand.ExecuteReaderAsync()
                    )
                    {
                        if (await reader.ReadAsync())
                        {
                            stock = reader.GetInt32(0);
                        }
                    }
                }

                string checkQuantityQuery =
                    "SELECT Quantity_Product FROM Cart WHERE Id_Cart = @id_Account AND Id_Product = @id";
                int quantity2 = 0;
                await using (
                    SqlCommand checkQuantityCommand = new SqlCommand(checkQuantityQuery, connection)
                )
                {
                    checkQuantityCommand.Parameters.AddWithValue(
                        "@id_Account",
                        HomeController.MainAccount.Id
                    );
                    checkQuantityCommand.Parameters.AddWithValue("@id", id);
                    await using (
                        SqlDataReader reader = await checkQuantityCommand.ExecuteReaderAsync()
                    )
                    {
                        if (await reader.ReadAsync())
                        {
                            quantity2 = reader.GetInt32(0);
                        }
                    }
                }

                if (quantity2 + quantity > stock)
                {
                    TempData["ErrorMessage"] = "Quantità non disponibile al momento";
                    return RedirectToAction("Detail", new { id = id });
                }

                string checkQuery =
                    "SELECT * FROM Cart WHERE Cart.Id_Product = @id AND Id_Cart = @account_id";
                await using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@id", id);
                    checkCommand.Parameters.AddWithValue(
                        "@account_id",
                        HomeController.MainAccount.Id
                    );
                    await using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reader.Close();
                            string updateQuery =
                                "UPDATE Cart SET Quantity_Product = Quantity_Product + @quantity WHERE Id_Product = @id AND Id_Cart = @account_id";
                            await using (
                                SqlCommand updateCommand = new SqlCommand(updateQuery, connection)
                            )
                            {
                                updateCommand.Parameters.AddWithValue("@quantity", quantity);
                                updateCommand.Parameters.AddWithValue("@id", id);
                                updateCommand.Parameters.AddWithValue(
                                    "@account_id",
                                    HomeController.MainAccount.Id
                                );
                                await updateCommand.ExecuteNonQueryAsync();
                            }

                            var newCartTwo = new CartController();
                            newCartTwo.GetCurrentCart();
                            return RedirectToAction("Detail", new { id = id });
                        }
                        reader.Close();
                    }
                }

                string query =
                    "INSERT INTO Cart (Id_Cart, Date_Add, Quantity_Product, Id_Product) VALUES (@account_id, @date_now, @quantity, @id_product)";
                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@account_id", HomeController.MainAccount.Id);
                    command.Parameters.AddWithValue("@date_now", DateTime.Now);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@id_product", id);
                    await command.ExecuteNonQueryAsync();
                }
            }

            var newCart = new CartController();
            newCart.GetCurrentCart();
            return RedirectToAction("Detail", new { id = id });
        }
    }
}
