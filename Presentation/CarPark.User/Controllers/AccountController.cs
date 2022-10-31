using AspNetCore.Identity.MongoDbCore.Models;
using CarPark.Entities.Concrete;
using CarPark.Models.RequestModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;

namespace CarPark.User.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Personel> _userManager;
        private readonly SignInManager<Personel> _signInManager;
        private readonly RoleManager<MongoIdentityRole> _roleManager;

        public AccountController(UserManager<Personel> userManager, SignInManager<Personel> signInManager, RoleManager<MongoIdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCreateModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new Personel
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = "5555555555"
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var role = new MongoIdentityRole
                    {
                        Name = "admin",
                        NormalizedName = "ADMIN"
                    };

                    await _roleManager.CreateAsync(role);
                    await _userManager.AddToRoleAsync(user, "admin");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToLocal(returnUrl);
                }
            }

            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
