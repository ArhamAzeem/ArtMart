using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ArtMart.Models;

namespace ArtMart.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
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


    }
}
