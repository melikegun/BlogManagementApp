using BlogManagementApp.Data;
using BlogManagementApp.Interfaces;
using BlogManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogManagementApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;

        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost, string userId)
        {
            blogPost.PublishedDate = DateTime.Now;
            blogPost.UserId = userId;

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await _context.BlogPosts
                .Include(b => b.Comments) 
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog != null)
            {
                _context.Comments.RemoveRange(blog.Comments); 
                _context.BlogPosts.Remove(blog);              
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(int id)
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.User)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            _context.BlogPosts.Update(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<BlogPost?> GetPreviousBlogAsync(int currentBlogId)
        {
            return await _context.BlogPosts
                .Where(b => b.Id < currentBlogId)
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<BlogPost?> GetNextBlogAsync(int currentBlogId)
        {
            return await _context.BlogPosts
                .Where(b => b.Id > currentBlogId)
                .OrderBy(b => b.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<BlogPost>> GetByUserIdAsync(string userId)
        {
            return await _context.BlogPosts
                .Include(b => b.Category)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
