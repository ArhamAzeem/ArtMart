using System;
using System.ComponentModel.DataAnnotations;

namespace ArtMart.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public string SellerId { get; set; }
        public ApplicationUser Seller { get; set; }

        public decimal Amount { get; set; }

        public DateTime PlacedAt { get; set; }

        public string Status { get; set; } 
    }

}
