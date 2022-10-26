using System.ComponentModel.DataAnnotations;

namespace Library.Data.Entities
{
    public class Book
    {

        public Book()
        {
            this.ApplicationUsersBooks = new HashSet<ApplicationUserBook>();
        }
        

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Author { get; set; }

        [Required]
        [StringLength(5000)]
        public string Description { get; set; }

        [Required,Url]
        public string ImageUrl { get; set; }

        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<ApplicationUserBook> ApplicationUsersBooks { get; set; }
    }
}
