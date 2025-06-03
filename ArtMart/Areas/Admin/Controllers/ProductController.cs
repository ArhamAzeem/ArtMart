using ArtMart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ArtMart.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace ArtMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : AdminBaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User session expired. Please login again.";
                return RedirectToAction("Login", "Auth");
            }

            var products = await _context.Products
                                         .Include(p => p.Seller)
                                         .Where(p => p.SellerId == userId)
                                         .ToListAsync();

            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            return View(products);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Product product)
        {
            // Check if a new image file is uploaded.
            var file = Request.Form.Files["ImageFile"];
            if (file != null && file.Length > 0)
            {
                // Create a unique file name.
                var fileName = Path.GetFileNameWithoutExtension(file.FileName)
                               + "_" + System.Guid.NewGuid().ToString()
                               + Path.GetExtension(file.FileName);

                var uploadDir = Path.Combine(_env.WebRootPath, "images", "products");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                product.ImageUrl = "/images/products/" + fileName;
            }
            else if (product.Id != 0)
            {
                // For an update, if no new file is uploaded, preserve the existing ImageUrl.
                var existingProduct = await _context.Products.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
            }
            // Get seller ID from session
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User session expired. Please login again.";
                return RedirectToAction("Login", "Auth");
            }
            product.SellerId = userId;

            if (product.Id == 0)
            {
                _context.Products.Add(product);
                TempData["SuccessMessage"] = "Product created successfully.";
            }
            else
            {
                // Update mode.
                _context.Products.Update(product);
                TempData["SuccessMessage"] = "Product updated successfully.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
            }
            else
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
