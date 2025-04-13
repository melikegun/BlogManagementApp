using BlogManagementApp.Data;
using BlogManagementApp.Interfaces;
using BlogManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogManagementApp.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string? userId = null)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null) return;

            if (userId == null || comment.UserId == userId) 
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.BlogPost)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
