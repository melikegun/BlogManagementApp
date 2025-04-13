using BlogManagementApp.Models;

namespace BlogManagementApp.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetAllAsync();                   // Tüm blogları getir
        Task<BlogPost> GetByIdAsync(int id);                  // ID ile blog getir
        Task<BlogPost> CreateAsync(BlogPost blogPost, string userId); // Blog oluştur
        Task<BlogPost> UpdateAsync(BlogPost blogPost);        // Blog güncelle
        Task DeleteAsync(int id);                             // Blog ve yorumlarını sil
        Task AddCommentAsync(Comment comment);                // Yorumu ekle
        Task<BlogPost?> GetPreviousBlogAsync(int currentBlogId); // Önceki blog
        Task<BlogPost?> GetNextBlogAsync(int currentBlogId);     // Sonraki blog
        Task<List<BlogPost>> GetByUserIdAsync(string userId); // Kullanıcının blogları
    }
}
