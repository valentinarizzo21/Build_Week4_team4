using System.Diagnostics;
using BeviSano.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BeviSano.Controllers;

public class HomeController : Controller
{
    public static Account? MainAccount { get; set; }

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

    public static CategoriesList categories = new CategoriesList();

    public async void GetCategories()
    {
        await using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            HomeController.categories.Categories = [];

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

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public async Task<IActionResult> Login(LoginViewModel loginData)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        Account account = new Account();
        bool accountFound = false;
        await using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query =
                "SELECT * FROM Account WHERE Email = @email AND Password_Account = @password";
            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", loginData.Email);
                command.Parameters.AddWithValue("@password", loginData.Password);
                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        account.Id = reader.GetGuid(0);
                        account.Name = reader.GetString(1);
                        account.Email = reader.GetString(2);
                        account.Password = reader.GetString(3);
                        account.admin = reader.GetBoolean(4);

                        accountFound = true;
                    }
                }
            }
        }
        if (!accountFound)
        {
            return View("index");
        }
        else
        {
            MainAccount = account;
            return RedirectToAction("Index", "Product");
        }
    }

    public IActionResult Logout()
    {
        MainAccount = null;

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddAccount(AddAccount addAccount)
    {
        if (!ModelState.IsValid)
        {
            return View("Register");
        }
        Account account = new Account();
        await using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string checkQuery = "SELECT * FROM Account WHERE Email = @email";
            await using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@email", addAccount.Email);
                await using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return View("Register");
                    }
                }
            }

            string query =
                "INSERT INTO Account (Name_Account, Email, Password_Account) OUTPUT INSERTED.Id_Account VALUES (@name, @email, @password)";
            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", addAccount.Name);
                command.Parameters.AddWithValue("@email", addAccount.Email);
                command.Parameters.AddWithValue("@password", addAccount.Password);
                var result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    account.Id = (Guid)result;
                    account.Name = addAccount.Name;
                    account.Email = addAccount.Email;
                    account.Password = addAccount.Password;
                    account.admin = false;
                }
            }
        }
        MainAccount = account;
        return RedirectToAction("Index", "Product");
    }

    public IActionResult FidelityCard()
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
