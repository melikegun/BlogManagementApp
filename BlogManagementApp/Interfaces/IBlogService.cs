using BlogManagementApp.Models;

namespace BlogManagementApp.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetAllAsync();
        Task<BlogPost> GetByIdAsync(int id);
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<BlogPost> UpdateAsync(BlogPost blogPost);
        Task DeleteAsync(int id);
    }
}
