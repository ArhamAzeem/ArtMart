using ArtMart.Controllers;
using ArtMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class CategoryController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    // New Category
                    _context.Categories.Add(category);
                    TempData["SuccessMessage"] = "Category created successfully.";
                }
                else
                {
                    // Update Existing
                    _context.Categories.Update(category);
                    TempData["SuccessMessage"] = "Category updated successfully.";
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            // In case of an error, re-display the page with the current model.
            TempData["ErrorMessage"] = "Failed Try Again.";
            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Category not found.";
            }
            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }

    }
}
