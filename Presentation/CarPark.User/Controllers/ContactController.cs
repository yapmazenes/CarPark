using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CarPark.User.Controllers
{
    public class ContactController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ContactController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            var welcome_Value = _localizer["Welcome"];
            return View();
        }
    }
}
