using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class DetailModel : PageModel
    {
        private readonly IRestaurantData restaurantData;

        public Restaurant Restaurant { get; set; }
        public DetailModel(IRestaurantData restaurantData)
        {
            this.restaurantData = restaurantData;
        }

        //public void OnGet(int restaurantId)
        public IActionResult OnGet(int restaurantId)
        {
            //Restaurant = new Restaurant();
            //Restaurant.Id = restaurantId; 
            Restaurant = restaurantData.GetRestaurantById(restaurantId);
            if (Restaurant == null)
            {
                return RedirectToPage("./NoFound");
            }
            return Page();

        }
    }
}
