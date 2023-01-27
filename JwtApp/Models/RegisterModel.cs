using System.ComponentModel.DataAnnotations;

namespace JwtApp.Models
{
    public class RegisterModel
    {
        [Required,MaxLength(128)]
        public string Email { get; set; }
        [Required, MaxLength(256)]
        public string Password { get; set; }
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(50)]
        public string UserName { get; set; }
    }
}
