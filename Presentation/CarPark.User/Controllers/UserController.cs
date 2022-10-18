using CarPark.User.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarPark.User.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserCreateRequestModel userCreateRequestModel)
        {
            return View();
        }
    }
}
