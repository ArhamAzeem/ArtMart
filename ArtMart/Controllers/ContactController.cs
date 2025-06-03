using ArtMart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMart.Controllers
{
    public class ContactController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        public ContactController(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage()
        {
            try
            {
                string name = Request.Form["Name"];
                string email = Request.Form["Email"];
                string messageContent = Request.Form["Message"];

                // Optional: Basic validation
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(messageContent))
                {
                    TempData["ErrorMessage"] = "All fields are required.";
                    return RedirectToAction("Index");
                }

                var message = new ContactMessage
                {
                    Name = name,
                    Email = email,
                    Message = messageContent,
                    CreatedAt = DateTime.Now // If you have this field
                };

                _dbContext.ContactMessages.Add(message);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you! Your message has been received.";
                return RedirectToAction("Index","Contact",new {area=""});
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index", "Contact", new { area = "" });
            }
        }
    }
}
