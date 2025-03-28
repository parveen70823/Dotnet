using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.R2
{
    public class DeleteModel : PageModel
    {
        private readonly OdeToFood.Data.OdeToFoodDbContext _context;

        public DeleteModel(OdeToFood.Data.OdeToFoodDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Restaurant Restaurant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(m => m.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }
            else
            {
                Restaurant = restaurant;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                Restaurant = restaurant;
                _context.Restaurants.Remove(Restaurant);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
