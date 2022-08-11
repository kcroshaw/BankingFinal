using Microsoft.AspNetCore.Mvc;

namespace Bank_EmpDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult FullTransactionHistory()
        {
            return View();
        }

        public IActionResult CheckingTransactionHistory()
        {
            return View();
        }

        public IActionResult SavingsTransactionHistory()
        {
            return View();
        }

        public IActionResult LoanTransactionHistory()
        {
            return View();
        }

    }
}
