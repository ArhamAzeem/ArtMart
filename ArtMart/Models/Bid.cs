using System.ComponentModel.DataAnnotations;

namespace ArtMart.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }

}
