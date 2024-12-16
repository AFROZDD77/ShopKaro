using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopKaro.Models.User
{
    public class User
    {
        [Key] // makes it the Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment Identity Column
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; } // Nullable Email, as User can chose either email or phonenumber
        public bool EmailConfirmed { get; set; }
        public int? PhoneNumber { get; set; } // Nullable PhoneNumber
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "erripooka")] // Ensure password has at least 10 characters
        public string Password { get; set; }
        public string Role { get; set; }
        public string City { get; set; }
    }
}
