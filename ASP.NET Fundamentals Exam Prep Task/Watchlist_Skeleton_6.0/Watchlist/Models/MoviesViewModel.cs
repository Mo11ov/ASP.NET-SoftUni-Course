﻿using Watchlist.Data.Entities;

namespace Watchlist.Models
{
    public class MoviesViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public decimal Rating { get; set; }

        public string ImageUrl { get; set; }

        public string Genre { get; set; }
    }
}
