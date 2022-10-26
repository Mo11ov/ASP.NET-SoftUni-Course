using System.ComponentModel.DataAnnotations;

namespace Library.Data.Entities
{
    public class Category
    {
        public Category()
        {
            this.Books = new HashSet<Book>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
