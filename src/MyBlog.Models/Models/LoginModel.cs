using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.Models
{
    public class LoginModel
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
