using Microsoft.AspNetCore.Identity;

namespace Library.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.ApplicationUsersBooks = new HashSet<ApplicationUserBook>();
        }
        
        public ICollection<ApplicationUserBook> ApplicationUsersBooks { get; set; }
    }
}
