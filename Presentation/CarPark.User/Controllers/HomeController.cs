using CarPark.User.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;

namespace CarPark.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> stringLocalizer)
        {
            _logger = logger;
            _localizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("First Log");

            var cultureInfo = CultureInfo.GetCultureInfo("en-US");

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var sayHelloValue = _localizer["say_Hello"];
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