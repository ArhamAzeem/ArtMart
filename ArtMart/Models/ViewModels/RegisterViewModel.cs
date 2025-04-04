using System.ComponentModel.DataAnnotations;

namespace ArtMart.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public required string FullName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public required string Password { get; set; }
        
        [Required,DataType(DataType.Password), Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
