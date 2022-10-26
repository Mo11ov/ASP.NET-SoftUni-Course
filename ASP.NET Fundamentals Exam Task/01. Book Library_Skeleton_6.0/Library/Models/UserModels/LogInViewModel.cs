using System.ComponentModel.DataAnnotations;

namespace Library.Models.UserModels
{
    public class LogInViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
