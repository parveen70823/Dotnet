using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]//by default this supoort for post operation
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }
        public EditModel(IRestaurantData restaurantData, IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }
        public IActionResult OnGet(int? restaurantId)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            if (restaurantId.HasValue)
            {
                Restaurant = restaurantData.GetRestaurantById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
            }
            if (Restaurant == null)
            {
                RedirectToPage("./NoFound");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();
            }
            if(Restaurant.Id > 0)
            {
                restaurantData.Update(Restaurant);
            }
            else
            {
                restaurantData.Add(Restaurant);
            }
            TempData["Message"] = "Restaurant saved!";//TempData is a dictionary that is stored in the session and is available for the next request and can access like a dictonary
            restaurantData.Commit();
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });//this pattern of redirecting after updatin in post request called post-redirect-get Pattern PRG pattern
        }
    }
}
