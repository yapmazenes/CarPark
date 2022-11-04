using AutoMapper;
using CarPark.Business.Abstract;
using CarPark.Entities.Concrete;
using CarPark.Models.ViewModels.Personels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace CarPark.User.Controllers
{
    [Authorize(Roles = "admin")]
    public class PersonelsController : Controller
    {
        private readonly IPersonelService _personelService;
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        private readonly UserManager<Personel> _userManager;
        private readonly IWebHostEnvironment _env;

        public PersonelsController(IPersonelService personelService, UserManager<Personel> userManager, ICityService cityService, IMapper mapper, IWebHostEnvironment env)
        {
            _personelService = personelService;
            _userManager = userManager;
            _cityService = cityService;
            _mapper = mapper;
            _env = env;
        }

        public IActionResult GetPersonelsByAge()
        {
            var result = _personelService.GetPersonelsByAge();

            return View(result);
        }

        public async Task<IActionResult> Settings()
        {
            var userTask = _userManager.GetUserAsync(User);
            var citiesTask = _cityService.GetAllCitiesAsync();

            await Task.WhenAll(userTask, citiesTask);

            var personelProfileInfo = _mapper.Map<PersonelProfileInfo>(await userTask);
            personelProfileInfo.Cities = (await citiesTask).Result;
            personelProfileInfo.ImageUrl = $"/Media/Personels/{personelProfileInfo.ImageUrl}";

            return View(personelProfileInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(PersonelProfileInfo personelProfileInfo)
        {

            var user = await _userManager.GetUserAsync(User);
            string imgUrl = "";

            if (personelProfileInfo.Image?.Length > 0)
            {
                var path = Path.Combine(_env.WebRootPath, "Media/Personels/");

                var fileName = $"{Guid.NewGuid()}_{personelProfileInfo.Image.FileName}";

                var filePath = Path.Combine(path, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    personelProfileInfo.Image.CopyTo(fileStream);
                    imgUrl = fileName;
                }
            }
            else
            {
                imgUrl = user.ImageUrl;
            }

            personelProfileInfo.UserName = user.UserName;
            personelProfileInfo.Email = user.Email;
            personelProfileInfo.ImageUrl = imgUrl;

            var userModel = _mapper.Map(personelProfileInfo, user);
            var identityResult = await _userManager.UpdateAsync(userModel);

            if (identityResult.Succeeded)
                return Json(new { message = "Basarili", success = true, personel = personelProfileInfo });
            else
                return Json(new { message = "Basarisiz", success = false, personel = personelProfileInfo });
        }

        [Route("getroles/{id}")]
        public async Task<IActionResult> GetRoles(string id)
        {
            var result = await _personelService.GetPersonelRoles(id);

            return Json(result);
        }

        [HttpPost]
        [Route("update/personel/roles")]
        public async Task<IActionResult> UpdatePersonelRoles(string personelId, string[] personelRoleList)
        {
            var updateResult = await _personelService.UpdatePersonelRoles(personelId, personelRoleList);

            return Json(updateResult);
        }
    }
}
