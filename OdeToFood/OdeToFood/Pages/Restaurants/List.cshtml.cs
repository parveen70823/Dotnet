using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;  //  Injects the configuration service to access settings from appsettings.json.
        private readonly IRestaurantData restaurantData;

        public string Message { get; set; }  // Property to store the message
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public ListModel(IConfiguration config, IRestaurantData restaurantData) // Constructor with Dependency Injection
        {
            this.config = config;
            this.restaurantData = restaurantData;
        }

        public void OnGet()  // This method is called when the page is accessed
        {
            Message = config["Message"]; // Reading "Message" from appsettings.json
            Restaurants = restaurantData.GetAll();
        }
    }
}
