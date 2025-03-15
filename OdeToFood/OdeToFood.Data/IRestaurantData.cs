using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
    }
    public class InMemoryRestaurantData : IRestaurantData
    {
        List<Restaurant> restaurants;
        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>()
            { 
                new Restaurant{Id = 1, Name = "Demo Pizza" , Location="new street,123", cuisine=  CuisineType.italian},
                new Restaurant{Id = 2, Name = "Demo Pasta" , Location="Maryland", cuisine=  CuisineType.maxican},
                new Restaurant{Id = 3, Name = "Demo Indian" , Location="Halkdja", cuisine=  CuisineType.indian},
            };

        }
        public IEnumerable<Restaurant> GetAll()
        {
            return from r in restaurants
                   orderby r.Name
                   select r;
        }
    }
}

