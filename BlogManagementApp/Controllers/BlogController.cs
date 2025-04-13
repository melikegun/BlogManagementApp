using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogManagementApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using BlogManagementApp.Interfaces;
using BlogManagementApp.Services;

namespace BlogManagementApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        private readonly IImageService _imageService;

        public BlogController(IBlogService blogService, ICommentService commentService, ICategoryService categoryService, UserManager<User> userManager, IImageService imageService)
        {
            _blogService = blogService;
            _commentService = commentService;
            _categoryService = categoryService;
            _userManager = userManager;
            _imageService = imageService;
        }



        [AllowAnonymous]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var allBlogs = await _blogService.GetAllAsync();
            var filteredBlogs = categoryId.HasValue
                ? allBlogs.Where(b => b.CategoryId == categoryId).ToList()
                : allBlogs;

            var categories = await _categoryService.GetAllAsync();

            var model = new BlogHomeViewModel
            {
                Categories = categories,
                BlogPosts = filteredBlogs,
                SelectedCategoryId = categoryId
            };

            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null) return NotFound();

            ViewBag.PreviousBlog = await _blogService.GetPreviousBlogAsync(id);
            ViewBag.NextBlog = await _blogService.GetNextBlogAsync(id);

            return View(blog);
        }


        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(BlogPost model, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    model.ImageUrl = await _imageService.UploadImageAsync(Image);
                }

                await _blogService.CreateAsync(model, _userManager.GetUserId(User));
                TempData["Success"] = "Blog başarıyla yayınlandı.";
                return RedirectToAction("Index");
            }

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            if (blog.UserId != currentUserId && !isAdmin)
                return Forbid();

            ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name", blog.CategoryId);
            return View(blog);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(BlogPost model, IFormFile? Image)
        {
            var existingBlog = await _blogService.GetByIdAsync(model.Id);
            if (existingBlog == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            if (existingBlog.UserId != currentUserId && !isAdmin)
                return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }

            existingBlog.Title = model.Title;
            existingBlog.Content = model.Content;
            existingBlog.CategoryId = model.CategoryId;

            if (Image != null && Image.Length > 0)
                existingBlog.ImageUrl = await _imageService.UploadImageAsync(Image);

            await _blogService.UpdateAsync(existingBlog);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string? returnUrl = null)
        {
            var blog = await _blogService.GetByIdAsync(id);
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            if (blog == null || (blog.UserId != currentUserId && !isAdmin))
                return Forbid();

            var comments = blog.Comments.ToList();
            foreach (var comment in comments)
            {
                await _commentService.DeleteAsync(comment.Id, null);
            }

            await _blogService.DeleteAsync(id);
            TempData["Success"] = "Blog ve yorumlar başarıyla silindi.";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }



        [Authorize]
        public async Task<IActionResult> MyBlogs()
        {
            var userId = _userManager.GetUserId(User);
            var blogs = await _blogService.GetByUserIdAsync(userId);
            return View(blogs);
        }


    }
}
