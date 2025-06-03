using System.ComponentModel.DataAnnotations;

namespace ArtMart.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } // FK to AspNetUsers or your custom User table
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
