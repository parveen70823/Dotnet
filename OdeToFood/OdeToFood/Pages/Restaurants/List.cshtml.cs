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
        private readonly ILogger<ListModel> logger;

        public string Message { get; set; }  // Property to store the message
        public IEnumerable<Restaurant> Restaurants { get; set; }

        [BindProperty(SupportsGet = true)]//asp.net method when this class instatiated this method always search for this parameter in Http request and populate itself automatically.
        public string SearchTerm { get; set; }
        public ListModel(IConfiguration config, IRestaurantData restaurantData,ILogger<ListModel> logger) // Constructor with Dependency Injection
        {
            this.config = config;
            this.restaurantData = restaurantData;
            this.logger = logger;
        }

        //public void OnGet(string searchTerm)  // This method is called when the page is accessed and in paras this is model binding.
        public void OnGet()  
        //Model Binding in ASP.NET Core automatically maps HTTP request data (like query strings, form inputs, and
        //route values) to C# properties or parameters. This eliminates the need for manual request parsing.
        {
            logger.LogError("Executing ListModel"); 
            //Message = config["Message"]; // Reading "Message" from appsettings.json
            //Restaurants = restaurantData.GetAll();
            Restaurants = restaurantData.GetRestaurantByName(SearchTerm);
        }
    }
}
