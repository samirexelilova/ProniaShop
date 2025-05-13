using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.Models;
using ProniaShop.Utilities.Enums;
using ProniaShop.Utilities.Extensions;
using ProniaShop.ViewModels;

namespace ProniaShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _roleManager=roleManager;
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
            await _userManager.AddToRoleAsync(appUser,UserRole.Member.ToString());
            await _signInManager.SignInAsync(appUser, false);
            return RedirectToAction(nameof(HomeController.Index),"Home");


        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM,string? returnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser user= await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName==loginVM.UsernameOrEmail || u.Email==loginVM.UsernameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty,"Username, Email ve ya Password sehvdir");
                return View();
            }
            var result=await _signInManager.PasswordSignInAsync(user, loginVM.Password,loginVM.IsPersistent,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Username, Email ve ya Password sehvdir");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Hesab bloklanib,10 deqiqe sonra yene cehd edin");
                return View();
            }
            if ( returnUrl is null) return RedirectToAction("Index","Home");


            return Redirect(returnUrl);

        }

        public async Task<IActionResult> Logout(LoginVM loginVM)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> CreateRoles()
        {
           
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name=role.ToString()
                });
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
