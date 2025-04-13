using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogManagementApp.Models;
using BlogManagementApp.Services;
using BlogManagementApp.Interfaces;
using BlogManagementApp.ViewModels;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IBlogService _blogService;
    private readonly ICommentService _commentService;

    public AdminController(UserManager<User> userManager, IBlogService blogService, ICommentService commentService)
    {
        _userManager = userManager;
        _blogService = blogService;
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var blogs = await _blogService.GetAllAsync();
        var comments = await _commentService.GetAllAsync();

        var model = new AdminDashboardViewModel
        {
            TotalUsers = users.Count,
            TotalBlogs = blogs.Count,
            TotalComments = comments.Count,
            RecentComments = comments
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .ToList()
        };

        return View(model);
    }


    public async Task<IActionResult> Comments()
    {
        var comments = await _commentService.GetAllAsync();
        return View(comments);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await _commentService.DeleteAsync(id, null);
        TempData["Success"] = "Yorum silindi.";
        return RedirectToAction("Comments");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecentComment(int id)
    {
        await _commentService.DeleteAsync(id, null);
        TempData["Success"] = "Yorum silindi.";
        return RedirectToAction("Index");
    }



}