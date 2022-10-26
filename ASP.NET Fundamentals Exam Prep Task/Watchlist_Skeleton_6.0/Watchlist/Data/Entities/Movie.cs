using System.ComponentModel.DataAnnotations;

namespace Watchlist.Data.Entities
{
    public class Movie
    {
        public Movie()
        {
            this.UsersMovies = new HashSet<UserMovie>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Director { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public decimal Rating { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<UserMovie> UsersMovies { get; set; }
    }
}
