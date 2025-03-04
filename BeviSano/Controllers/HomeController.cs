using System.Diagnostics;
using BeviSano.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BeviSano.Controllers;

public class HomeController : Controller
{
    public static CategoriesList categories = new CategoriesList();

    private readonly string _connectionString;

    public HomeController()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        _connectionString = configuration.GetConnectionString("DefaultConnection");

        GetCategories();
    }

    public async void GetCategories()
    {
        await using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM Categories;";

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var category = new Category()
                        {
                            Id_Category = reader.GetInt32(0),
                            Title = reader.GetString(1),
                        };

                        HomeController.categories.Categories.Add(category);
                    }
                }
            }
        }
    }

    private readonly ILogger<HomeController> _logger;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
