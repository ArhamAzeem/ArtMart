using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArtMart.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        // Additional Profile Fields
        public DateTime? DateOfBirth { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        [Required]
        public string Role { get; set; } = "Customer";
    }
}
