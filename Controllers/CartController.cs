using FoodApp.FoodDb;
using FoodApp.Models;
using FoodApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IData _data;
        private readonly FoodAppDbContext _context;
        public CartController(IData data,FoodAppDbContext context)
        {
            this._data = data;
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _data.GetUser(HttpContext.User);
            var cartlist = await _context.Carts.Where(x => x.UserId == user.Id).ToListAsync();
            return View(cartlist);
        }

        [HttpPost]

        public async Task<IActionResult> SaveCart(Cart cart)
        {
            var user = await  _data.GetUser(HttpContext.User);
            cart.UserId = user?.Id;
            if (ModelState.IsValid)
            {
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
                TempData["CartAdd"] = "Recipe Added Successfully to cart";
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task <IActionResult> GetAddedCarts()
        {
            var user = await _data.GetUser(HttpContext.User);
            var carts = _context.Carts.Where(c => c.UserId == user.Id).Select(c => c.RecipeId).ToList();
            return Ok (carts);
        }
        [HttpPost]
        public async Task <IActionResult> RemoveCart(string Id) 
        {
            if (!string.IsNullOrEmpty(Id)) 
            {
                var cart =  _context.Carts.Where(x => x.RecipeId == Id).FirstOrDefault();
                if (cart != null) 
                {
                     _context.Carts.Remove(cart);
                     _context.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var user = await  _data.GetUser(HttpContext.User);
            var cartitem =  _context.Carts.Where(x => x.UserId == user.Id).Take(3).ToList();
            return PartialView("_CartList",cartitem);
        }
    }
}
