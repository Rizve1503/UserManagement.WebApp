using System.ComponentModel.DataAnnotations;

namespace UserManagement.WebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime LastLoginTimeUtc { get; set; }

        public DateTime RegistrationTimeUtc { get; set; }

        public bool IsBlocked { get; set; }
    }
}
