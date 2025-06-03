using System;
using System.Linq;
using System.Threading.Tasks;
using ArtMart.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtMart.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CheckAndSendBiddingEndNotifications()
        {
            var now = DateTime.Now;

            var endedProducts = await _context.Products
                .Where(p => p.BiddingEndTime < now)
                .ToListAsync();

            foreach (var product in endedProducts)
            {
                bool alreadyNotified = await _context.Notifications.AnyAsync(n =>
                    n.UserId == product.SellerId &&
                    n.Message.Contains($"Bidding ended for '{product.Name}'"));

                if (alreadyNotified) continue;

                var highestBid = await _context.Bids
                    .Where(b => b.ProductId == product.Id)
                    .OrderByDescending(b => b.Amount)
                    .FirstOrDefaultAsync();

                if (highestBid != null)
                {
                    _context.Notifications.AddRange(
                        new Notification
                        {
                            UserId = product.SellerId,
                            Message = $"Bidding ended for '{product.Name}'. Highest bid: Rs. {highestBid.Amount}",
                            CreatedAt = now
                        },
                        new Notification
                        {
                            UserId = highestBid.UserId,
                            Message = $"You won the bid for '{product.Name}' with Rs. {highestBid.Amount}. " +
              $"<a href='/Orders/Create?productId={product.Id}' class='btn btn-sm btn-success'>Place Order</a>",
                            CreatedAt = now
                        }
                    );
                }
                else
                {
                    _context.Notifications.Add(new Notification
                    {
                        UserId = product.SellerId,
                        Message = $"Bidding ended for '{product.Name}' with no bids.",
                        CreatedAt = now
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
