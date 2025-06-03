using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ArtMart.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtMart.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        public AccountController(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = await _dbContext.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return View(notifications);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ApplicationUser model, string CurrentPassword, string NewPassword, string ConfirmNewPassword)
        {
            if (!ModelState.IsValid)
                return View("Profile", model); // Explicitly return the Profile view

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Auth", new { area = "" });

            // Update profile details
            user.FullName = model.FullName;
            user.DateOfBirth = model.DateOfBirth;
            user.ContactNumber = model.ContactNumber;
            user.Address = model.Address;
            user.Country = model.Country;
            user.City = model.City;
            user.State = model.State;
            user.Zip = model.Zip;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["ErrorMessage"] = "Profile update failed. Please check the errors and try again.";
                return View("Profile", model);
            }

            // Process password change if any password fields are provided
            if (!string.IsNullOrWhiteSpace(CurrentPassword) ||
                !string.IsNullOrWhiteSpace(NewPassword) ||
                !string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                // All fields must be provided for a password change
                if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                    string.IsNullOrWhiteSpace(NewPassword) ||
                    string.IsNullOrWhiteSpace(ConfirmNewPassword))
                {
                    ModelState.AddModelError("", "To update your password, please fill out all password fields.");
                    TempData["ErrorMessage"] = "Password update failed. To update your password, please fill out all password fields.";
                    return View("Profile", model);
                }

                if (NewPassword != ConfirmNewPassword)
                {
                    ModelState.AddModelError("", "New password and confirmation do not match.");
                    TempData["ErrorMessage"] = "Password update failed. New password and confirmation do not match.";
                    return View("Profile", model);
                }

                // Attempt to change the password
                var passwordResult = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    TempData["ErrorMessage"] = "Password update failed. Please check the errors and try again.";
                    return View("Profile", model);
                }
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Profile", "Account", new { area = "" });
        }

        [HttpGet]
        public async Task<IActionResult> RequestList()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Auth", new { area = "" });

            // Retrieve this user's promotion requests
            var requests = _dbContext.PromotionRequests
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.RequestDate)
                .ToList();

            return View(requests);
        }

        public async Task<IActionResult> SendPromotionRequest()
        {
            var userId = _userManager.GetUserId(User);
            var existingRequest = await _dbContext.PromotionRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestDate)
                .FirstOrDefaultAsync();

            if (existingRequest != null && existingRequest.Status == PromotionStatus.Pending)
            {
                TempData["ErrorMessage"] = "You already have a pending promotion request.";
                return RedirectToAction("RequestList", "Account", new { area = "" });
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            var request = new PromotionRequest
            {
                UserId = userId,
                RequestDate = DateTime.UtcNow,
                Status = PromotionStatus.Pending
            };

            #pragma warning restore CS8601 // Possible null reference assignment.
            _dbContext.PromotionRequests.Add(request);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your request to become an Artist has been submitted.";
            return RedirectToAction("RequestList", "Account", new { area = "" });
        }

        public async Task<IActionResult> MyBids()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var myBids = _dbContext.Bids
            .Include(b => b.Product)
            .Where(b => b.UserId == user.Id)
            .OrderByDescending(b => b.Id)
            .ToList();

            return View(myBids);
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var orders = await _dbContext.Orders
                .Include(o => o.Product)
                .Include(o => o.Seller)
                .Where(o => o.CustomerId.ToString() == user.Id) // Convert CustomerId to string for comparison  
                .ToListAsync();

            return View(orders);
        }

    }
}
