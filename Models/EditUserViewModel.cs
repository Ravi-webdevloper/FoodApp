using System.ComponentModel;

namespace FoodApp.Models
{
    public class EditUserViewModel:RegisterViewModel
    {
        [ReadOnly (true)]
        public string? Id { get; set; }
        public string ? UserName { get; set; }
    }
}
