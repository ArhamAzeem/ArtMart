using System.Security.Claims;
using ArtMart.Controllers;
using ArtMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace ArtMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    //[Authorize(Roles = "Admin")]
    public class DashboardController : AdminBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public DashboardController(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Promote(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
            }

            // Check if user already has the Admin role
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["ErrorMessage"] = "User is already an Admin!";
                return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                // Update the custom Role property if you use it in your views.
                user.Role = "Admin";
                await _userManager.UpdateAsync(user);
                TempData["SuccessMessage"] = "User promoted to Admin!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to promote user!";
            }

            return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Update the custom Role property if you use it in your views.
                user.Role = "Admin";
                await _userManager.UpdateAsync(user);
                TempData["SuccessMessage"] = "User Deleted Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete User!";
            }

            return RedirectToAction("Users", "Dashboard", new { area = "Admin" });
        }

        public async Task<IActionResult> Requests()
        {
            // Retrieve only pending requests
            var pendingRequests = await _dbContext.PromotionRequests
                .Include(r => r.User)
                .Where(r => r.Status == PromotionStatus.Pending)
                .OrderBy(r => r.RequestDate)
                .ToListAsync();

            return View(pendingRequests);
        }

        [HttpGet("Admin/PromotionRequests/Accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {
            var request = await _dbContext.PromotionRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                TempData["ErrorMessage"] = "Request not found.";
                return RedirectToAction("Index");
            }

            // Update the request status
            request.Status = PromotionStatus.Accepted;
            request.ResponseMessage = "Your request has been accepted.";
            _dbContext.PromotionRequests.Update(request);
            await _dbContext.SaveChangesAsync();

            // Promote the user to Artist if not already
            var user = request.User;
            if (!await _userManager.IsInRoleAsync(user, "Artist"))
            {
                // Optionally, remove Customer role if necessary
                if (await _userManager.IsInRoleAsync(user, "Customer"))
                    await _userManager.RemoveFromRoleAsync(user, "Customer");

                await _userManager.AddToRoleAsync(user, "Artist");
                user.Role = "Artist";
                await _userManager.UpdateAsync(user);
            }

            TempData["SuccessMessage"] = "Request accepted. User is now an Artist.";
            return RedirectToAction("Index");
        }

        // GET: /Admin/PromotionRequests/Reject/{id}
        [HttpGet("Admin/PromotionRequests/Reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var request = await _dbContext.PromotionRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                TempData["ErrorMessage"] = "Request not found.";
                return RedirectToAction("Index");
            }

            // Update the request status to Rejected
            request.Status = PromotionStatus.Rejected;
            request.ResponseMessage = "Your request has been rejected. Please try again later.";
            _dbContext.PromotionRequests.Update(request);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request rejected.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Messages()
        {
            var messages = await _dbContext.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return View(messages);
        }

        public async Task<IActionResult> SellerOrders()
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _dbContext.Orders
                .Include(o => o.Product)
                .Where(o => o.SellerId == sellerId)
                .OrderByDescending(o => o.PlacedAt)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) return NotFound();

            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.SellerId != sellerId) return Forbid();

            order.Status = newStatus;
            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Status Updated!";

            return RedirectToAction("SellerOrders");
        }
    }
}
