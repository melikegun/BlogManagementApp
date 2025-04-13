using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogManagementApp.Interfaces;
using BlogManagementApp.Models;

namespace BlogManagementApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public CommentController(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(int blogPostId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                TempData["Error"] = "Yorum boş olamaz.";
                return RedirectToAction("Details", "Blog", new { id = blogPostId });
            }

            var comment = new Comment
            {
                BlogPostId = blogPostId,
                Text = commentText,
                UserId = _userManager.GetUserId(User),
                CreatedAt = DateTime.Now
            };

            await _commentService.AddCommentAsync(comment);
            TempData["Success"] = "Yorum başarıyla eklendi.";
            return RedirectToAction("Details", "Blog", new { id = blogPostId });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _commentService.DeleteAsync(id, userId);

            return RedirectToAction("Index", "Blog");
        }
    }
}
