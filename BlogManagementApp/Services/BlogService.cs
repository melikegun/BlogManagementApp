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

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog != null)
            {
                _context.BlogPosts.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPosts.Include(b => b.Category).Include(b => b.User).ToListAsync();
        }

        public async Task<BlogPost> GetByIdAsync(int id)
        {
            return await _context.BlogPosts.Include(b => b.Category).Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            _context.BlogPosts.Update(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }
    }
}
