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
        //IEnumerable<Restaurant> GetAll();
        IEnumerable<Restaurant> GetRestaurantByName(string name);
        Restaurant GetRestaurantById(int id);
        Restaurant Update(Restaurant updatedRestaurant);
        int Commit(); 
    }
    public class InMemoryRestaurantData : IRestaurantData
    {
        List<Restaurant> restaurants;
        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>()
            { 
                new Restaurant{Id = 1, Name = "La Pizza" , Location="new street,123", cuisine=  CuisineType.italian},
                new Restaurant{Id = 2, Name = "Scott's Pasta" , Location="Maryland", cuisine=  CuisineType.maxican},
                new Restaurant{Id = 3, Name = "In Indian" , Location="Halkdja", cuisine=  CuisineType.indian},
            };

        }
        //public IEnumerable<Restaurant> GetAll()
        //{
        //    return from r in restaurants
        //           orderby r.Name
        //           select r;
        //}
        public Restaurant GetRestaurantById(int id)
        {
            return restaurants.SingleOrDefault(s => s.Id == id);
        }
        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if (restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.cuisine = updatedRestaurant.cuisine;
            }
            return restaurant;
        }
        public int Commit()
        {
            return 0;
        }
        public IEnumerable<Restaurant> GetRestaurantByName(string name = null)
        {
            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }
    }
}

