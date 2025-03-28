using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data
{
    public class OdeToFoodDbContext:DbContext
    {
        public OdeToFoodDbContext(DbContextOptions<OdeToFoodDbContext> options):base(options)
        {
            
        }
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
