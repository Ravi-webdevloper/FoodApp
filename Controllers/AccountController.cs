using Microsoft.AspNetCore.Mvc;
using FoodApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FoodApp.Repository;
using Microsoft.AspNetCore.Authorization;
namespace FoodApp.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IData _data;
        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signinmanager,IPasswordHasher<ApplicationUser> passwordHasher,IData data)
        {
            _userManager = usermanager;
            _signInManager = signinmanager;
            _passwordHasher = passwordHasher;
            _data = data;
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

        public async Task<IActionResult> Update()
         {
            
            var userId =  _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                EditUserViewModel ViewModel = new EditUserViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    UserName = user.Email,
                    Password = user.PasswordHash,
                    
                };
              
                return View(ViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditUserViewModel ViewModel)
        {

            if(ModelState.IsValid)
            {
                
                ApplicationUser user = await _userManager.FindByIdAsync(ViewModel.Id);
                if(user != null)
                {
                    //user.Id = ViewModel.Id;
                    user.Name = ViewModel.Name;
                    user.Email = ViewModel.Email;
                    user.Address = ViewModel.Address;
                    user.PasswordHash = _passwordHasher.HashPassword(user,ViewModel.Password);
                    user.UserName = ViewModel.UserName;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["Update Data"] = "Data Updated Successfully";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return View();
                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                }
            }

            return View();
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
        public async Task <IActionResult> Register(RegisterViewModel register)
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
