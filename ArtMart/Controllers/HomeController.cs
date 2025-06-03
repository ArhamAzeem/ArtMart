using System.Diagnostics;
using ArtMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMart.Controllers
{
    //[Authorize(Roles = "Customer, Artist")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the latest 7 live products, ordered by bidding start (or any field you prefer)
            var liveProducts = await _context.Products
                .Include(p => p.Seller)
                .Where(p => p.Status == ProductStatus.Live)
                .OrderByDescending(p => p.BiddingStartTime)
                .Take(7)
                .ToListAsync();

            // Get the upcoming products, ordered by bidding start time, and take 7 (or adjust as needed)
            var upcomingProducts = await _context.Products
                .Include(p => p.Seller)
                .Where(p => p.Status == ProductStatus.Upcoming)
                .OrderBy(p => p.BiddingStartTime)
                .Take(7)
                .ToListAsync();

            ViewBag.LiveProducts = liveProducts;
            ViewBag.UpcomingProducts = upcomingProducts;
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
