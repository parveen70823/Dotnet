using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Core
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required, StringLength(80)]//.net provide some common model validation which validate the input data in this model
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Location { set; get; }

        public CuisineType cuisine { set; get; }

    }
}
