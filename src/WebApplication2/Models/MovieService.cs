using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class MovieService
    {
        public MovieService()
        {
            Movies = new List<Movie>
            {
                new Movie { ID = 1, Title = "La grande vadrouille" },
                new Movie { ID = 2, Title = "Le corniaud" }
            };
        }

        public List<Movie> Movies { get; set; }

        internal void SaveChanges()
        {
            
        }
    }
}