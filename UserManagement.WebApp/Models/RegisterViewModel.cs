using System.ComponentModel.DataAnnotations;

namespace UserManagement.WebApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        // Per HR: "User can use any non-empty password (even one character)".
        // So we don't need a minimum length here, just that it's required.
        public string Password { get; set; } = string.Empty;
    }
}
