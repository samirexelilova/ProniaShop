using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaShop.Models;
using ProniaShop.Utilities.Extensions;
using ProniaShop.ViewModels;

namespace ProniaShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager=userManager;
            _signInManager=signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (registerVM.Name.NormalizeName() == null || registerVM.Surname.NormalizeName() == null)
            {
                ModelState.AddModelError(nameof(RegisterVM.Name), "Ad ve Soyad yalniz herfden ibaret olmalidir");
                return View();
            }
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name.NormalizeName(),
                Surname = registerVM.Surname.NormalizeName(),
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };
            IdentityResult result= await _userManager.CreateAsync(appUser,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
                return View();
            }

            await _signInManager.SignInAsync(appUser, false);
            return RedirectToAction(nameof(HomeController.Index),"Home");


        }
    }
}
