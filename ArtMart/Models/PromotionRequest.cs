using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMart.Models
{
    public enum PromotionStatus
    {
        Pending,
        Accepted,
        Rejected
    }
    public class PromotionRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        public PromotionStatus Status { get; set; } = PromotionStatus.Pending;

        public string? ResponseMessage { get; set; }
    }
}
