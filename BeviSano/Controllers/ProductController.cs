using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}
