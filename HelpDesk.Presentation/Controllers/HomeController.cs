using HelpDesk.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HelpDesk.BAL.Services;

namespace HelpDesk.Presentation.Controllers
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
            TestService service = new TestService();

            var data = service.GetSample();

            ViewBag.Id = data.Id;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       

    }
}

