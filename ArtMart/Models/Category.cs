using System.ComponentModel.DataAnnotations;

namespace ArtMart.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
    }

}
