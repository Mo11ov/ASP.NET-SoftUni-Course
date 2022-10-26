using Library.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.BookModels
{
    public class AddBookViewModel
    {
        public AddBookViewModel()
        {
            this.Categories = new HashSet<Category>();
        }


        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Author { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public string Description { get; set; }

        [Required, Url]
        public string ImageUrl { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0", "10.0", ConvertValueInInvariantCulture = true)]
        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
