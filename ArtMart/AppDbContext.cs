using System.Reflection.Emit;
using ArtMart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtMart
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add DbSet for PromotionRequest
        public DbSet<PromotionRequest> PromotionRequests { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" },
                new IdentityRole { Name = "Artist", NormalizedName = "ARTIST" }
            );

            // Optionally, configure relationships or property constraints for PromotionRequest if needed.
            builder.Entity<PromotionRequest>()
                .HasOne(pr => pr.User)
                .WithMany()  // You can specify a collection property on ApplicationUser if you want a navigation property.
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
