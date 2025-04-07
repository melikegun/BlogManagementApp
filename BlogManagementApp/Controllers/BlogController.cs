using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogManagementApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogManagementApp.Data;

namespace BlogManagementApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public BlogController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var blogPosts = _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.User)
                .OrderByDescending(b => b.PublishedDate)
                .ToList();

            return View(blogPosts);
        }


        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var blog = _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.User)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(b => b.Id == id);

            if (blog == null) return NotFound();

            return View(blog);
        }



        [Authorize]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(BlogPost model, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder); // klasör yoksa oluştur

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    model.ImageUrl = "/uploads/" + uniqueFileName;
                }

                model.PublishedDate = DateTime.Now;
                model.UserId = _userManager.GetUserId(User);

                _context.BlogPosts.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(model);
        }



        [Authorize]
        public IActionResult Edit(int id)
        {
            var blog = _context.BlogPosts.FirstOrDefault(b => b.Id == id);
            if (blog == null || blog.UserId != _userManager.GetUserId(User))
            {
                return Forbid(); // veya return NotFound();
            }

            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", blog.CategoryId);

            return View(blog);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(BlogPost model, IFormFile? Image)
        {
            var blog = _context.BlogPosts.FirstOrDefault(b => b.Id == model.Id);
            if (blog == null || blog.UserId != _userManager.GetUserId(User))
            {
                return NotFound(); 
            }

            if (ModelState.IsValid)
            {
                blog.Title = model.Title;
                blog.Content = model.Content;
                blog.CategoryId = model.CategoryId;

                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    blog.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
            return View(model);
        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            var blog = _context.BlogPosts
                .Include(b => b.User)
                .FirstOrDefault(b => b.Id == id);

            if (blog == null || blog.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(blog);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            var blog = _context.BlogPosts.FirstOrDefault(b => b.Id == id);
            if (blog == null || blog.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            _context.BlogPosts.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int blogPostId, string commentText)
        {
            var comment = new Comment
            {
                BlogPostId = blogPostId,
                Text = commentText,
                UserId = _userManager.GetUserId(User),
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = blogPostId });
        }




    }
}
