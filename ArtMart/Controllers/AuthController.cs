using ArtMart.Models;
using ArtMart.Models.ViewModels;
using ArtMart.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtMart.Controllers
{
    public class AuthController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NotificationService _notificationService;

        public AuthController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
            NotificationService notificationService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _notificationService = notificationService;
        }

        //Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Register Method
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Role = "Customer" };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer"); // Assign Role
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    SetUserSession(user); // Store user info in session

                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            await _notificationService.CheckAndSendBiddingEndNotifications();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        SetUserSession(user); // Store user info in session

                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        else
                            return RedirectToAction("Index", "Account", new { area = "" });
                    }
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private void SetUserSession(ApplicationUser user)
        {
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("FullName", user.FullName ?? "");
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role);
        }

    }
}
