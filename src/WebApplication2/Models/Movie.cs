﻿using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace WebApplication2.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }

        public List<string> Actors { get; set; }
    }

    public class MovieList// : DbContext
    {
        public List<Movie> Movies { get; set; }
    }
}