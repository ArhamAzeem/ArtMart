using ArtMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArtMart.Controllers
{
    public class AuctionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuctionController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Auction/Gallery
        // Accepts filters and sortOrder; no pagination is applied.
        public async Task<IActionResult> Gallery(
            string[]? statuses,
            int[]? categories,
            string[]? artists,
            decimal? minPrice,
            decimal? maxPrice,
            string sortOrder = "latest")
        {
            // Start query with eager loading of related entities.
            var query = _context.Products
                                .Include(p => p.Seller)
                                .Include(p => p.Category)
                                .AsQueryable();

            // Apply filters.
            if (statuses != null && statuses.Any())
            {
                var statusEnums = statuses.Select(s => (ProductStatus)Enum.Parse(typeof(ProductStatus), s, true));
                query = query.Where(p => statusEnums.Contains(p.Status));
            }

            if (categories != null && categories.Any())
            {
                query = query.Where(p => categories.Contains(p.CategoryId));
            }

            // Filter by artist using seller full name.
            if (artists != null && artists.Any())
            {
                query = query.Where(p => artists.Contains(p.Seller.FullName));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.BiddingStartPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.BiddingStartPrice <= maxPrice.Value);
            }

            // Sorting logic.
            switch (sortOrder.ToLower())
            {
                case "latest":
                    query = query.OrderByDescending(p => p.BiddingStartTime);
                    break;
                case "priceasc":
                    query = query.OrderBy(p => p.BiddingStartPrice);
                    break;
                case "pricedesc":
                    query = query.OrderByDescending(p => p.BiddingStartPrice);
                    break;
                default:
                    query = query.OrderByDescending(p => p.BiddingStartTime);
                    break;
            }

            var products = await query.ToListAsync();

            // Populate filter options.
            ViewBag.AllStatuses = Enum.GetNames(typeof(ProductStatus));
            ViewBag.AllCategories = await _context.Categories.ToListAsync();
            var artistUsers = await _userManager.GetUsersInRoleAsync("Artist");
            ViewBag.AllArtists = artistUsers.Select(u => u.FullName)
                                            .Where(n => !string.IsNullOrEmpty(n))
                                            .Distinct()
                                            .ToList();

            // Pass along current filters and sort order.
            ViewBag.FilteredStatuses = statuses;
            ViewBag.FilteredCategories = categories;
            ViewBag.FilteredArtists = artists;
            ViewBag.MinPrice = minPrice ?? 0;
            ViewBag.MaxPrice = maxPrice ?? 1000;
            ViewBag.SortOrder = sortOrder;

            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            // Fetch the highest bid
            var highestBid = await _context.Bids
                .Where(b => b.ProductId == id)
                .OrderByDescending(b => b.Amount)
                .Select(b => b.Amount)
                .FirstOrDefaultAsync();

            // Pass the highest bid to the view using ViewBag or a ViewModel
            ViewBag.HighestBid = highestBid > 0 ? highestBid : product.BiddingStartPrice;

            return View(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceBid(int productId, decimal bidAmount)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            var latestBid = _context.Bids
                .Where(b => b.ProductId == productId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefault();

            decimal minAllowedBid = latestBid?.Amount + 1 ?? product.BiddingStartPrice + 1;

            if (bidAmount < minAllowedBid)
            {
                TempData["ErrorMessage"] = $"Minimum allowed bid is ${minAllowedBid:F2}";
                return RedirectToAction("Details", "Auction", new { area = "", id = productId });
            }

            var userId = _userManager.GetUserId(User);

            var bid = new Bid
            {
                ProductId = productId,
                Amount = bidAmount,
                UserId = userId,
                PlacedAt = DateTime.UtcNow
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your bid was placed successfully!";
            return RedirectToAction("Details","Auction", new { area = "",id = productId });
        }

    }
}
