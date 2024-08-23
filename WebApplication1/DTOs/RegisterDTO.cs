using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "The two passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
