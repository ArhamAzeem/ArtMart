using System.Security.Claims;
using ArtMart.Models;
using ArtMart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMart.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return NotFound();

            var highestBid = await _context.Bids
                .Where(b => b.ProductId == productId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync();

            if (highestBid == null || highestBid.UserId != GetCurrentUserId().ToString())
                return Forbid();

            var viewModel = new OrderPlacementViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                SellerName = product.Seller.FullName,
                Amount = highestBid.Amount
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderPlacementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check your payment details.";
                return View(model);
            }

            var product = await _context.Products
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == model.ProductId);

            var highestBid = await _context.Bids
                .Where(b => b.ProductId == model.ProductId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync();

            if (product == null || highestBid == null)
                return NotFound();

            // ✅ Mark product as Sold
            product.Status = ProductStatus.Sold;

            var order = new Order
            {
                ProductId = product.Id,
                SellerId = product.SellerId,          // Already a string
                CustomerId = highestBid.UserId,       // Already a string
                Amount = highestBid.Amount,
                PlacedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Orders.Add(order);

            // ✅ Update product status in DB
            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Success","Orders", new {area="", id = order.Id });
        }

        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Seller)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || order.CustomerId != GetCurrentUserId())
                return Forbid();

            return View(order);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
