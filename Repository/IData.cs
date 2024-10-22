using FoodApp.Models;
using System.Security.Claims;

namespace FoodApp.Repository
{
    public interface IData
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims);
    }
}
