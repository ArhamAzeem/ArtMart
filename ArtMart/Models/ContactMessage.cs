﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ArtMart.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
