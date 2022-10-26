using System.ComponentModel.DataAnnotations;

namespace Watchlist.Data.Entities
{
    public class Genre
    {
        public Genre()
        {
            this.Movies = new HashSet<Movie>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
