using BlogManagementApp.Data;
using BlogManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogManagementApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CommentController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            if (comment == null || comment.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            int blogPostId = comment.BlogPostId;
            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Blog", new { id = blogPostId });
        }
    }
}
