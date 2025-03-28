using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class DeleteModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        public Restaurant Restaurant { get; set; }
        public DeleteModel(IRestaurantData restaurantData)
        {
            this.restaurantData = restaurantData;
        }
        public void OnGet(int restaurantId)
        {
            Restaurant = restaurantData.GetRestaurantById(restaurantId);
        }
        public IActionResult OnPost(int restaurantId)
        {
            var restaurant = restaurantData.Delete(restaurantId);
            restaurantData.Commit();
            if (restaurant == null)
            {
                return RedirectToPage("./NoFound");
            }
            TempData["Message"] = $"{restaurant.Name} deleted!";
            return RedirectToPage("./List");
        }
    }
}
