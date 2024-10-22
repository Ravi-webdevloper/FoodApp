using Microsoft.AspNetCore.Mvc;
using FoodApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FoodApp.FoodDb;
namespace FoodApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FoodAppDbContext _context;
        public RecipesController(UserManager<ApplicationUser> userManager,FoodAppDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetRecipecard([FromBody] List<Recipe> recipes)
        {
            return PartialView("_RecipeCard", recipes);
        }
        public IActionResult Search([FromQuery] string recipe)
        {
            ViewBag.Recipes = recipe;
            return View();
        }

        public IActionResult Order([FromQuery] string id)
        {
            ViewBag.Id = id;
            
            return View();
        }

        public async Task<IActionResult> ShowOrder(OrderRecipeDetails details)
        {
            Random rand = new Random();
            ViewBag.Price = Math.Round(rand.Next(300, 600)/5.0)*5;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user?.Id;
            ViewBag.Address = user?.Address;
            return PartialView("_ShowOrder", details);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Order([FromForm]Order order)
        {
            order.OrderDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Recipes");
            }
            return RedirectToAction("Order", "Recipes", new {id=order.Id});
        }
    }
}
