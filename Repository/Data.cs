using FoodApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodApp.Repository
{
    public class Data : IData
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        public Data(UserManager<ApplicationUser> manager)
        {
            _usermanager = manager;
        }
        public async Task<ApplicationUser> GetUser(ClaimsPrincipal claims)
        {
            return await _usermanager.GetUserAsync(claims);
        }
    }
}
