using BeviSano.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BeviSano.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _connectionString;

        public AdminController()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            var productsList = new ProductsListAdminView();
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
                            productsList.Products.Add(product);
                        }
                    }
                }
            }

            return View(productsList);
        }

        [HttpPost("/admin/add/save")]
        public async Task<IActionResult> AddProduct(AddProduct newProduct)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var myProduct = new Product();

            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query =
                    "INSERT INTO Products (Id_Product, Name_Product, Price_Product, Description_Product, Stock_Product, Seller_Product, Sale_Product, Arrival_Date_Product, Cover_Product, Id_Category) VALUES (@newId, @name, @price, @description, @stock, @seller, @sale, @date, @cover, @idCategory);";

                if (newProduct.Image_One != null)
                {
                    query =
                        query
                        + " INSERT INTO Images (Id_Image, Id_Product, Url_Image) VALUES (@imageOneId, @productId1, @imageOneUrl);";
                }

                if (newProduct.Image_Two != null)
                {
                    query =
                        query
                        + " INSERT INTO Images (Id_Image, Id_Product, Url_Image) VALUES (@imageTwoId, @productId2, @imageTwoUrl);";
                }

                if (newProduct.Image_Three != null)
                {
                    query =
                        query
                        + " INSERT INTO Images (Id_Image, Id_Product, Url_Image) VALUES (@imageThreeId, @productId3, @imageThreeUrl);";
                }

                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var newId = Guid.NewGuid();

                    command.Parameters.AddWithValue("@newId", newId);
                    command.Parameters.AddWithValue("@name", newProduct.Name_Product);
                    command.Parameters.AddWithValue("@price", newProduct.Price_Product);
                    command.Parameters.AddWithValue("@description", newProduct.Description_Product);
                    command.Parameters.AddWithValue("@stock", newProduct.Stock_Product);
                    command.Parameters.AddWithValue("@seller", newProduct.Seller_Product);
                    command.Parameters.AddWithValue("@sale", newProduct.Sale_Product);
                    command.Parameters.AddWithValue("@date", newProduct.Arrival_Date_Product);
                    command.Parameters.AddWithValue("@cover", newProduct.Cover_Product);
                    command.Parameters.AddWithValue("@idCategory", newProduct.Id_Category);

                    if (newProduct.Image_One != null)
                    {
                        command.Parameters.AddWithValue("@imageOneId", Guid.NewGuid());
                        command.Parameters.AddWithValue("@productId1", newId);
                        command.Parameters.AddWithValue("@imageOneUrl", newProduct.Image_One);
                    }

                    if (newProduct.Image_Two != null)
                    {
                        command.Parameters.AddWithValue("@imageTwoId", Guid.NewGuid());
                        command.Parameters.AddWithValue("@productId2", newId);
                        command.Parameters.AddWithValue("@imageTwoUrl", newProduct.Image_Two);
                    }

                    if (newProduct.Image_Three != null)
                    {
                        command.Parameters.AddWithValue("@imageThreeId", Guid.NewGuid());
                        command.Parameters.AddWithValue("@productId3", newId);
                        command.Parameters.AddWithValue("@imageThreeUrl", newProduct.Image_Three);
                    }

                    int righeInteressate = await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpGet("/admin/edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var selectedProduct = new EditProduct();

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
                            selectedProduct.Id_Product = reader.GetGuid(0);
                            selectedProduct.Name_Product = reader.GetString(1);
                            selectedProduct.Price_Product = decimal.Parse(
                                reader.GetDecimal(2).ToString("0.00")
                            );
                            selectedProduct.Description_Product = reader.GetString(3);
                            selectedProduct.Stock_Product = reader.GetInt32(4);
                            selectedProduct.Seller_Product = reader.GetString(5);
                            selectedProduct.Sale_Product = decimal.Parse(
                                reader.GetDecimal(6).ToString("0")
                            );
                            selectedProduct.Arrival_Date_Product = reader.GetInt32(7);
                            selectedProduct.Cover_Product = reader.GetString(8);
                            selectedProduct.Id_Category = reader.GetInt32(9);
                        }
                    }
                }

                string queryImgs = "SELECT * FROM Images WHERE Id_Product = @IdProduct";
                await using (SqlCommand commandImgs = new SqlCommand(queryImgs, connection))
                {
                    commandImgs.Parameters.AddWithValue("@IdProduct", id);

                    await using (SqlDataReader reader = await commandImgs.ExecuteReaderAsync())
                    {
                        List<Image> Images = new List<Image>();

                        while (await reader.ReadAsync())
                        {
                            var img = new Image()
                            {
                                Id_Image = reader.GetGuid(0),
                                Url_Image = reader.GetString(2),
                            };

                            Images.Add(img);
                        }

                        switch (Images.Count)
                        {
                            case 1:
                                selectedProduct.Id_Image_One = Images[0].Id_Image;
                                selectedProduct.Image_One = Images[0].Url_Image;
                                break;

                            case 2:
                                selectedProduct.Id_Image_One = Images[0].Id_Image;
                                selectedProduct.Image_One = Images[0].Url_Image;
                                selectedProduct.Id_Image_Two = Images[1].Id_Image;
                                selectedProduct.Image_Two = Images[1].Url_Image;
                                break;

                            case 3:
                                selectedProduct.Id_Image_One = Images[0].Id_Image;
                                selectedProduct.Image_One = Images[0].Url_Image;
                                selectedProduct.Id_Image_Two = Images[1].Id_Image;
                                selectedProduct.Image_Two = Images[1].Url_Image;
                                selectedProduct.Id_Image_Three = Images[2].Id_Image;
                                selectedProduct.Image_Three = Images[2].Url_Image;
                                break;

                            case > 3:
                                selectedProduct.Id_Image_One = Images[0].Id_Image;
                                selectedProduct.Image_One = Images[0].Url_Image;
                                selectedProduct.Id_Image_Two = Images[1].Id_Image;
                                selectedProduct.Image_Two = Images[1].Url_Image;
                                selectedProduct.Id_Image_Three = Images[2].Id_Image;
                                selectedProduct.Image_Three = Images[2].Url_Image;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            return View(selectedProduct);
        }

        [HttpPost("/admin/edit/save/{id:guid}")]
        public async Task<IActionResult> SaveEdit(Guid id, EditProduct editedProduct)
        {
            editedProduct.Id_Product = id;
            editedProduct.Id_Image_One = (Guid?)TempData["Id_Image_One"];
            editedProduct.Id_Image_Two = (Guid?)TempData["Id_Image_Two"];
            editedProduct.Id_Image_Three = (Guid?)TempData["Id_Image_Three"];

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var myProduct = new Product();

            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query =
                    $"UPDATE Products SET Name_Product=@name, Price_Product=@price, Description_Product=@description, Stock_Product=@stock, Seller_Product=@seller, Sale_Product=@sale, Arrival_Date_Product=@date, Cover_Product=@cover, Id_Category=@idCategory WHERE Id_Product=@id";

                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name", editedProduct.Name_Product);
                    command.Parameters.AddWithValue("@price", editedProduct.Price_Product);
                    command.Parameters.AddWithValue(
                        "@description",
                        editedProduct.Description_Product
                    );
                    command.Parameters.AddWithValue("@stock", editedProduct.Stock_Product);
                    command.Parameters.AddWithValue("@seller", editedProduct.Seller_Product);
                    command.Parameters.AddWithValue("@sale", editedProduct.Sale_Product);
                    command.Parameters.AddWithValue("@date", editedProduct.Arrival_Date_Product);
                    command.Parameters.AddWithValue("@cover", editedProduct.Cover_Product);
                    command.Parameters.AddWithValue("@idCategory", editedProduct.Id_Category);

                    int interestedRows = await command.ExecuteNonQueryAsync();
                }

                List<Image> Images = new List<Image>();

                if (editedProduct.Image_One != null && editedProduct.Image_One != "")
                {
                    Images.Add(
                        new Image()
                        {
                            Url_Image = editedProduct.Image_One,
                            Id_Image =
                                editedProduct.Id_Image_One != null
                                    ? editedProduct.Id_Image_One
                                    : null,
                        }
                    );
                }

                if (editedProduct.Image_Two != null && editedProduct.Image_Two != "")
                {
                    Images.Add(
                        new Image()
                        {
                            Url_Image = editedProduct.Image_Two,
                            Id_Image =
                                editedProduct.Id_Image_Two != null
                                    ? editedProduct.Id_Image_Two
                                    : null,
                        }
                    );
                }

                if (editedProduct.Image_Three != null && editedProduct.Image_Three != "")
                {
                    Images.Add(
                        new Image()
                        {
                            Url_Image = editedProduct.Image_Three,
                            Id_Image =
                                editedProduct.Id_Image_Three != null
                                    ? editedProduct.Id_Image_Three
                                    : null,
                        }
                    );
                }

                foreach (var img in Images)
                {
                    string queryImgs;

                    if (img.Id_Image == null)
                    {
                        queryImgs =
                            "INSERT INTO Images VALUES (@Id_Image, @Id_Product, @Url_Image);";

                        await using (SqlCommand command = new SqlCommand(queryImgs, connection))
                        {
                            command.Parameters.AddWithValue("@Id_Image", Guid.NewGuid());
                            command.Parameters.AddWithValue("@Id_Product", id);
                            command.Parameters.AddWithValue("@Url_Image", img.Url_Image);

                            int interestedRows = await command.ExecuteNonQueryAsync();
                        }
                    }
                    else
                    {
                        queryImgs =
                            "UPDATE Images SET Id_Product=@IdProduct, Url_Image=@UrlImage WHERE Id_Image=@idImage;";

                        await using (SqlCommand command = new SqlCommand(queryImgs, connection))
                        {
                            command.Parameters.AddWithValue("@idImage", img.Id_Image);
                            command.Parameters.AddWithValue("@IdProduct", id);
                            command.Parameters.AddWithValue("@UrlImage", img.Url_Image);

                            int interestedRows = await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet("/admin/delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM Products WHERE Id_Product = @id;";

                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int interestedRows = await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
