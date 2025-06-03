using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace ArtMart.Models
{
    public enum ProductStatus
    {
        Upcoming,
        Live,
        Sold
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }

        // A user-friendly identifier for the product.
        [Required]
        public required string ProductSpecificId { get; set; }

        [Required]
        public required string Name { get; set; }

        // Artist Name as a free text field (this may come from the seller’s input)
        [Required]
        public string ArtistName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal BiddingStartPrice { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BiddingStartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BiddingEndTime { get; set; }

        [Required]
        public required string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string? ArtistDOB { get; set; }
        public string? ArtistDeath { get; set; }     
        public string? Nationality { get; set; }
        public string? Style { get; set; }
        public string? NotableWork { get; set; }

        // Exploring Artwork
        public string? Size { get; set; }
        public string? Framing { get; set; }
        public string? Theme { get; set; }
        public string? Signature { get; set; }
        public string? Authenticity { get; set; }

        // Art Inner Narrative and Guidelines
        public string? ArtInnerNarrative { get; set; }
        public string? Guidelines { get; set; }

        // Bidding status
        [Required]
        public ProductStatus Status { get; set; } = ProductStatus.Upcoming;

        // Record which seller (user) added the product
        [Required]
        public string SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual ApplicationUser Seller { get; set; }
    }
}
