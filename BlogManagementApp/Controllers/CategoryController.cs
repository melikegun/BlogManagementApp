using Microsoft.AspNetCore.Mvc;
using BlogManagementApp.Models;
using BlogManagementApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BlogManagementApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _categoryService.CreateAsync(category);
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _categoryService.UpdateAsync(category);
            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasBlogs = await _categoryService.HasAnyBlogAsync(id);

            if (hasBlogs)
            {
                TempData["Error"] = "❗ Bu kategoriye bağlı bloglar bulunduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }

            await _categoryService.DeleteAsync(id);
            TempData["Success"] = "Kategori başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }



    }
}
