using Microsoft.AspNetCore.Mvc;
using FoodApp.Models;
using Microsoft.AspNetCore.Identity;
namespace FoodApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signinmanager)
        {
            _userManager = usermanager;
            _signInManager = signinmanager;
        }
       
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string? returnurl)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = login.Email,
                };
                var result = await _signInManager.PasswordSignInAsync(user.Email,login.Password,false,false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnurl))
                        return LocalRedirect(returnurl);
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError(" ", "Invalid Login Attempt");
            }
            return View(login);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register( RegisterViewModel register)
        {
            if (ModelState.IsValid) 
            {
                var user = new ApplicationUser
                {
                    Name = register.Name,
                    Address = register.Address,
                    Email = register.Email,
                    UserName = register.Email
                };
                var result = await _userManager.CreateAsync(user,register.Password);
                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(user, register.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(register);
        }
    }
}
